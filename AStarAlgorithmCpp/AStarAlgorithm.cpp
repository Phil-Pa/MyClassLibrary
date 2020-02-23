#include "AStarAlgorithm.h"

#include <windows.h>

struct Tile {
	Tile* previos;
	int x, y, type;
	float f, g, h;
	Tile* neighbors[8];

	void AddNeighbors(const int* values, int size, bool diagonal) {

		int neighborIndex = 0;

		
	}
};

int FindTile(const int const* values, const int tile) {
	int index = 0;
	while (true) {
		if (values[index] == tile)
			return index;
		++index;
	}
}

void NativeAStarAlgorithm(int* values, const int size, bool diagonal)
{

	values[0] = 42;
	//MessageBox(HWND_DESKTOP, L"Bin in der DLL!", L"Hinweis", MB_OK);

	int startIndex = FindTile(values, START);
	int endIndex = FindTile(values, END);

	Tile* tiles = new Tile[size];

	int index = 0;
	for (int x = 0; x < size; x++)
	{
		for (int y = 0; y < size; y++)
		{
			Tile tile = Tile{ nullptr, x, y, values[index], 0.0f, 0.0f, 0.0f };
			tile.AddNeighbors(values, size, diagonal);
			tiles[index] = tile;

			++index;
		}
	}

	delete[] tiles;
}
