using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace MyClassLibrary.Windows
{
	public class GlobalKeyboardHook : IDisposable
	{
		private enum WM : uint
		{
			KEYDOWN = 0x0100
		}

		[StructLayout(LayoutKind.Sequential)]
		public class KBDLLHOOKSTRUCT
		{
			public uint vkCode;
			public uint scanCode;
			public KBDLLHOOKSTRUCTFlags flags;
			public uint time;
			public UIntPtr dwExtraInfo;
		}

		[Flags]
		public enum KBDLLHOOKSTRUCTFlags : uint
		{
			LLKHF_EXTENDED = 0x01,
			LLKHF_INJECTED = 0x10,
			LLKHF_ALTDOWN = 0x20,
			LLKHF_UP = 0x80,
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;

			public POINT(int x, int y)
			{
				X = x;
				Y = y;
			}

			public static implicit operator System.Drawing.Point(POINT p)
			{
				return new System.Drawing.Point(p.X, p.Y);
			}

			public static implicit operator POINT(System.Drawing.Point p)
			{
				return new POINT(p.X, p.Y);
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MSG
		{
			private readonly IntPtr hwnd;
			private readonly uint message;
			private readonly UIntPtr wParam;
			private readonly IntPtr lParam;
			private readonly int time;
			private POINT pt;
		}

		public event Action<int>? OnKeyDown = null;

		static GlobalKeyboardHook()
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				throw new PlatformNotSupportedException("hooks are only available on windows");
			}
		}

		public enum HookType : int
		{
			WH_JOURNALRECORD = 0,
			WH_JOURNALPLAYBACK = 1,
			WH_KEYBOARD = 2,
			WH_GETMESSAGE = 3,
			WH_CALLWNDPROC = 4,
			WH_CBT = 5,
			WH_SYSMSGFILTER = 6,
			WH_MOUSE = 7,
			WH_HARDWARE = 8,
			WH_DEBUG = 9,
			WH_SHELL = 10,
			WH_FOREGROUNDIDLE = 11,
			WH_CALLWNDPROCRET = 12,
			WH_KEYBOARD_LL = 13,
			WH_MOUSE_LL = 14
		}

		private delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("user32.dll")]
		private static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

		[DllImport("user32.dll")]
		private static extern bool TranslateMessage([In] ref MSG lpMsg);

		[DllImport("user32.dll")]
		private static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

		[DllImport("user32.dll")]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, WM wParam, [In]KBDLLHOOKSTRUCT lParam);

		private readonly object _lock = new object();

		private bool shouldStop = false;

		private readonly Thread thread;

		private IntPtr hHook;

		public GlobalKeyboardHook()
		{
			thread = new Thread(Keylogger)
			{
				IsBackground = true,
				Name = "Keylogger Thread"
			};
		}

		private IntPtr LowLevelKeyboardProc(int code, IntPtr wParam, IntPtr lParam)
		{
			if (code >= 0 && wParam == (IntPtr)WM.KEYDOWN)
			{
				var kbd = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

				var handler = OnKeyDown;
				handler?.Invoke((int)kbd.vkCode);
			}
			return CallNextHookEx(hHook, code, wParam, lParam);
		}

		private void Keylogger()
		{
			var hExe = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
			hHook = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, LowLevelKeyboardProc, hExe, 0);

			while (GetMessage(out var msg, IntPtr.Zero, 0, 0) != 0)
			{
				lock (_lock)
				{
					if (shouldStop)
					{
						break;
					}
				}

				TranslateMessage(ref msg);
				DispatchMessage(ref msg);
			}

			UnhookWindowsHookEx(hHook);

			Debug.WriteLine(thread.Name + " has left the Keylogger() proc");
		}

		public void Start()
		{
			Debug.WriteLine(thread.Name + " started");
			thread.Start();
		}

		public void Stop()
		{
			lock (_lock)
			{
				shouldStop = true;
			}
			Debug.WriteLine(thread.Name + " should stop");
		}

		#region IDisposable Support

		private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: verwalteten Zustand (verwaltete Objekte) entsorgen.
				}

				// TODO: nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer weiter unten überschreiben.
				UnhookWindowsHookEx(hHook);
				// TODO: große Felder auf Null setzen.
				hHook = IntPtr.Zero;

				Debug.WriteLine("Hook is disposed");

				disposedValue = true;
			}
		}

		// TODO: Finalizer nur überschreiben, wenn Dispose(bool disposing) weiter oben Code für die Freigabe nicht verwalteter Ressourcen enthält.
		~GlobalKeyboardHook()
		{
			// Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
			Dispose(false);
		}

		// Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
		void IDisposable.Dispose()
		{
			// Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
			Dispose(true);
			// TODO: Auskommentierung der folgenden Zeile aufheben, wenn der Finalizer weiter oben überschrieben wird.
			GC.SuppressFinalize(this);
		}

		#endregion IDisposable Support
	}
}