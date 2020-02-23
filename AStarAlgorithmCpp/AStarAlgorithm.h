#pragma once

#define WALL 1
#define WALKABLE 2
#define PATH 3
#define START 4
#define END 5
#define TEST 42

extern "C" __declspec(dllexport) void NativeAStarAlgorithm(int* values, const int size, bool diagonal);