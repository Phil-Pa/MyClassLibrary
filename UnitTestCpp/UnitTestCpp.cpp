#include "pch.h"
#include "CppUnitTest.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace UnitTestCpp
{
	TEST_CLASS(UnitTestCpp)
	{
	public:
		
		TEST_METHOD(TestAStarDiagonal)
		{

			int values[]{
				WALKABLE, WALKABLE, WALKABLE, WALKABLE,
				START,    WALL,     WALL,     WALL,
				WALKABLE, WALL,     END,      WALKABLE,
				WALKABLE, WALKABLE, WALKABLE, WALKABLE
			};

			int result[]{
				WALKABLE, WALKABLE, WALKABLE, WALKABLE,
				START,    WALL,     WALL,     WALL,
				PATH,     WALL,     PATH,     WALKABLE,
				WALKABLE, PATH,     WALKABLE, WALKABLE
			};

			NativeAStarAlgorithm(values, 16, true);

			Assert::IsTrue(std::equal(std::begin(values), std::end(values), std::begin(result)));
		}

		TEST_METHOD(TestAStarNonDiagonal) {
			int values[]{
					WALKABLE, WALKABLE, WALKABLE, WALKABLE,
					START,    WALL,     WALL,     WALL,
					WALKABLE, WALL,     END,      WALKABLE,
					WALKABLE, WALKABLE, WALKABLE, WALKABLE
			};

			int result[]{
				WALKABLE, WALKABLE, WALKABLE, WALKABLE,
				START,    WALL,     WALL,     WALL,
				PATH,     WALL,     PATH,     WALKABLE,
				PATH,     PATH,     PATH,     WALKABLE
			};

			NativeAStarAlgorithm(values, 16, false);

			Assert::IsTrue(std::equal(std::begin(values), std::end(values), std::begin(result)));
		}

		TEST_METHOD(TestAStarAreNeighbors) {

			// tests for non diagonal

			Assert::IsTrue(TestAreNeighbors(0, 1, 6, false));
			Assert::IsTrue(TestAreNeighbors(0, 6, 6, false));

			Assert::IsFalse(TestAreNeighbors(0, -1, 6, false));
			Assert::IsFalse(TestAreNeighbors(0, 7, 6, false));
			Assert::IsFalse(TestAreNeighbors(6, 5, 6, false));
			Assert::IsFalse(TestAreNeighbors(29, 30, 6, false));

			Assert::IsTrue(TestAreNeighbors(1, 0, 6, false));
			Assert::IsTrue(TestAreNeighbors(1, 7, 6, false));
			Assert::IsTrue(TestAreNeighbors(1, 2, 6, false));

			Assert::IsTrue(TestAreNeighbors(22, 16, 6, false));
			Assert::IsTrue(TestAreNeighbors(22, 23, 6, false));
			Assert::IsTrue(TestAreNeighbors(22, 21, 6, false));
			Assert::IsTrue(TestAreNeighbors(22, 28, 6, false));

			Assert::IsFalse(TestAreNeighbors(22, 29, 6, false));

			// tests for diagonal

			Assert::IsTrue(TestAreNeighbors(0, 1, 6, true));
			Assert::IsTrue(TestAreNeighbors(0, 6, 6, true));
			Assert::IsTrue(TestAreNeighbors(0, 7, 6, true));

			Assert::IsFalse(TestAreNeighbors(0, -1, 6, true));
			Assert::IsTrue(TestAreNeighbors(0, 7, 6, true));
			Assert::IsFalse(TestAreNeighbors(6, 5, 6, true));
			Assert::IsFalse(TestAreNeighbors(29, 30, 6, true));

			Assert::IsTrue(TestAreNeighbors(1, 0, 6, true));
			Assert::IsTrue(TestAreNeighbors(1, 7, 6, true));
			Assert::IsTrue(TestAreNeighbors(1, 2, 6, true));

			Assert::IsTrue(TestAreNeighbors(22, 16, 6, true));
			Assert::IsTrue(TestAreNeighbors(22, 23, 6, true));
			Assert::IsTrue(TestAreNeighbors(22, 21, 6, true));
			Assert::IsTrue(TestAreNeighbors(22, 28, 6, true));
			Assert::IsTrue(TestAreNeighbors(22, 15, 6, true));
			Assert::IsTrue(TestAreNeighbors(22, 17, 6, true));
			Assert::IsTrue(TestAreNeighbors(22, 27, 6, true));
			Assert::IsTrue(TestAreNeighbors(22, 29, 6, true));

			Assert::IsTrue(TestAreNeighbors(22, 29, 6, true));
		}
	};
}
