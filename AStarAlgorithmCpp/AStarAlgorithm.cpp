#include "AStarAlgorithm.h"

#include <cmath>
#include <cstring>

// disable malloc returns null check
#pragma warning(push)
#pragma warning(disable: 6387)

inline int min(int a, int b) {
	return a > b ? b : a;
}

inline int max(int a, int b) {
	return a < b ? b : a;
}

// the input tiles, also serving as the output tiles
static int* tiles;

// if diagonal movement is allowed
static bool diag;

// index of the start/end tile, and the length of the tiles array
static int startIndex, endIndex, len;

// the tiles array is an 2d array flattened to a 1d array. the 2d array has the size of rowLen * rowLen
static int rowLen;

// buffer, for the current best indices, marked with x
// x x x
// x * x
// x x x
// ...
static int* neighborBuffer;

// number of currently stored neighbors
static int nbLen = 0;

// storing the g/h-cost of tile at index
static int* gCostBuffer;
static int* hCostBuffer;

// array containing the indices of the prefered tiles to look at next
static int* pickNextBuffer;

// used length of the pickNextBuffer
static int pickNextLen = 0;

// in the pickNextBuffer are multiple tiles (associated with the index).
// this array stores the tiles (indices) with the same hCost
static int* minHCostBuffer;

// used length of the array
static int minHCostLen = 0;

// multiplied with the measured cost, to distinguish the measured value
#define COST_SCALE 10

// parameter is the tile type, like path, wall, etc.
int FindTile(const int tile) {
	int index = 0;
	while (true) {
		if (tiles[index] == tile)
			return index;
		++index;
	}
}

// returns the hypot from indexFrom to tileTo
int GetCostToTile(int indexFrom, int tileTo) {

	int tileToCopy = tileTo;
	int indexFromCopy = indexFrom;

	// TODO: what if not diagonal?

	// if (diag)

	int ySteps = 0;

	if (abs(tileTo - indexFrom) > rowLen) {
		while (indexFrom > tileTo + rowLen)
		{
			++ySteps;
			indexFrom -= rowLen;
		}

		while (tileTo > indexFrom + rowLen)
		{
			++ySteps;
			tileTo -= rowLen;
		}
	}
	else {
		// TODO: refactor
		if (abs(indexFrom - tileTo) < rowLen)
			ySteps = abs(indexFrom - tileTo);
	}

	if (indexFrom == tileTo)
		return 0;

	int xSteps = abs((tileToCopy - indexFromCopy) / rowLen);
	return hypot(xSteps, ySteps) * COST_SCALE;

	// else
}

int GetGCost(int index) {
	return GetCostToTile(index, startIndex);
}

int GetHCost(int index) {
	return GetCostToTile(index, endIndex);
}

int GetFCost(int index) {
	return gCostBuffer[index] + hCostBuffer[index];
}

// tile indices a and b
bool AreNeighbors(int a, int b) {

	if (a < 0 || b < 0)
		return false;

	int diff = abs(a - b);

	if (rowLen == diff)
		return true;

	// wie oft passt a bzw. b in rowLen rein?

	if (a / rowLen == b / rowLen && a >= rowLen && b >= rowLen && diff == 1)
		return true;

	if (diag) {

		a %= rowLen;
		b %= rowLen;

		if (abs(a - b) == 1)
			return true;
		else
			return false;
	}
	else {

		// if a and b are like:
		// x x x a
		// b x x x
		// x x x x
		// x x x x

		if (abs(a - b) == 1 && (a / rowLen > b / rowLen || b / rowLen > a / rowLen))
			return false;

		// if a is above or under b
		if (a % rowLen == b % rowLen)
			return true;
		else if (abs(a - b) == 1)
			return true;
		else
			return false;
	}
}

bool TestAreNeighbors(int a, int b, int rowLen, bool diag) {

	if (a < 0 || b < 0)
		return false;

	int diff = abs(a - b);

	if (rowLen == diff)
		return true;

	// wie oft passt a bzw. b in rowLen rein?

	if (a / rowLen == b / rowLen && a >= rowLen && b >= rowLen && diff == 1)
		return true;

	if (diag) {

		a %= rowLen;
		b %= rowLen;

		if (abs(a - b) == 1)
			return true;
		else
			return false;
	}
	else {
		
		// if a and b are like:
		// x x x a
		// b x x x
		// x x x x
		// x x x x

		if (abs(a - b) == 1 && (a / rowLen > b / rowLen || b / rowLen > a / rowLen))
			return false;

		// if a is above or under b
		if (a % rowLen == b % rowLen)
			return true;
		else if (abs(a - b) == 1)
			return true;
		else
			return false;

	}
}

bool IsWall(int index) {
	return tiles[index] == WALL;
}

void UpdateNeighborBufferAroundTile(int tileIndex) {

	// indices contains the indices of the adjacent tiles of tileIndex

	int indices[8];
	int index = 0;

	indices[index++] = tileIndex - rowLen - 1;
	indices[index++] = indices[0] + 1;
	indices[index++] = indices[0] + 2;
	indices[index++] = indices[0] + rowLen;
	indices[index++] = indices[3] + 2;
	indices[index++] = indices[3] + rowLen;
	indices[index++] = indices[5] + 1;
	indices[index++] = indices[5] + 2;

	nbLen = 0;

	// filter the indices, because tiles at the matrix edge dont have adjacent tiles in every direction

	for (int i = 0; i < 8; i++)
	{
		if (indices[i] >= 0 && AreNeighbors(indices[i], tileIndex) && !IsWall(indices[i]))
			neighborBuffer[nbLen++] = indices[i];
	}

	for (int i = 0; i < nbLen; i++)
	{
		gCostBuffer[i] = GetGCost(neighborBuffer[i]);
		hCostBuffer[i] = GetHCost(neighborBuffer[i]);
	}
}

int GetMinFCostTile() {

	int minValue = INT_MAX;

	for (int i = 0; i < nbLen; i++)
	{
		minValue = min(minValue, GetHCost(i));
	}

	int indexResult = 0;

	for (int i = 0; i < nbLen; i++)
	{
		if (minValue == GetHCost(i)) {
			indexResult = neighborBuffer[i];
			break;
		}
	}

	return indexResult;

	// TODO: what if 2 tiles have the same f cost?

	//int minValue = INT_MAX;
	//for (int index = 0; index < len; index++)
	//{
	//	int fCost = GetFCost(index);

	//	// already reached the min value, but 2 tiles have the same min value
	//	if (minValue == fCost) {
	//		pickNextBuffer[pickNextLen++] = index;
	//	}

	//	minValue = min(minValue, fCost);
	//}

	//--pickNextLen;
	//if (pickNextLen < 0)
	//	return -1;
	//else
	//	return pickNextBuffer[pickNextLen];
}

int PickNextTile() {
	//int minFCostTile = INT_MAX;
	//int last = -1;
	//while (minFCostTile != -1) {
	//	

	//	if (minFCostTile == -1)
	//		return last;

	//	last = minFCostTile;
	//}

	static int i = 0;

	int minFCostTile = GetMinFCostTile();

	for (;i < pickNextLen; i++)
	{
		if (GetFCost(minFCostTile) == GetFCost(i) && minFCostTile != i) {
			
			// only compares 2 hCosts, should return minHCosts of all tiles which have the same minFCost
			return GetHCost(minFCostTile) > GetHCost(i) ? i : minFCostTile;
		}
	}

	return minFCostTile;
}

void InitializeValues(int* values, const int size, bool diagonal) {
	tiles = values;

	// len/size is meant as the whole size, do not something like len * len
	len = size;
	rowLen = (int)sqrt(len);

	diag = diagonal;

	// optimize and do only 1 malloc call like malloc(len * numArrays), and set the array pointers to ptr = &<malloc result>(numArray * len)

	// optimize with good approximation
	neighborBuffer = (int*)malloc(sizeof(int) * len);
	gCostBuffer = (int*)malloc(sizeof(int) * len);
	hCostBuffer = (int*)malloc(sizeof(int) * len);
	pickNextBuffer = (int*)malloc(sizeof(int) * len);
	minHCostBuffer = (int*)malloc(sizeof(int) * len);

	memset(neighborBuffer, -1, sizeof(int) * len);
	memset(gCostBuffer, -1, sizeof(int) * len);
	memset(hCostBuffer, -1, sizeof(int) * len);
}

void CleanupValues() {
	free(neighborBuffer);
	free(gCostBuffer);
	free(hCostBuffer);
	free(pickNextBuffer);
	free(minHCostBuffer);
}

void NativeAStarAlgorithm(int* values, const int size, bool diagonal)
{
	InitializeValues(values, size, diagonal);

	startIndex = FindTile(START);
	endIndex = FindTile(END);

	int currentTileIndex = startIndex;

	while (currentTileIndex != endIndex) {
		UpdateNeighborBufferAroundTile(currentTileIndex);

		currentTileIndex = PickNextTile();

		tiles[currentTileIndex] = TEST;
	}

	CleanupValues();
}

#pragma warning(pop)