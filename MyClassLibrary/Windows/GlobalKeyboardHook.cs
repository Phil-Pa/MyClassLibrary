using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MyClassLibrary.Windows
{
	public class GlobalKeyboardHook : IDisposable
	{

		private static GlobalKeyboardHook? _instance;

		public static void Create(Action<Key> callback)
		{
			_instance = new GlobalKeyboardHook(callback);
		}

		public static GlobalKeyboardHook Instance
		{
			get
			{
				if (_instance == null)
					throw new Exception("Create must be called before accessing the instance");

				return _instance;
			}
		}

		public enum Key
		{
			Escape = 0x1B,
			Tab = 0x09,
			Space = 0x20
		}

		private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

		// ReSharper disable once InconsistentNaming
		private const int WH_KEYBOARD_LL = 13;

		// ReSharper disable once InconsistentNaming
		private const int WM_KEYDOWN = 0x0100;

		static GlobalKeyboardHook()
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				throw new Exception("program is not running on windows");
			}
		}

		private readonly Action<Key> _callback;

		private GlobalKeyboardHook(Action<Key> callback)
		{
			_callback = callback;
			SetHook(Proc);
		}

		private static readonly LowLevelKeyboardProc Proc = HookCallback;
		private static readonly IntPtr HookId = IntPtr.Zero;

		private static void SetHook(LowLevelKeyboardProc proc)
		{
			using Process curProcess = Process.GetCurrentProcess();
			using ProcessModule curModule = curProcess.MainModule;
			if (curModule != null)
				SetWindowsHookEx(WH_KEYBOARD_LL, proc,
					GetModuleHandle(curModule.ModuleName), 0);
			else
			{
				throw new Exception("internal hook error");
			}
		}

		private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
			{
				var vkCode = Marshal.ReadInt32(lParam);

				Key key = (Key) vkCode;

				Instance._callback(key);
			}

			return CallNextHookEx(HookId, nCode, wParam, lParam);
		}

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		private static void ReleaseUnmanagedResources()
		{
			UnhookWindowsHookEx(HookId);
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~GlobalKeyboardHook()
		{
			ReleaseUnmanagedResources();
		}
	}
}
