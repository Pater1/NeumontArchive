#pragma once
#include <vector>

#include "Character.h"
#include "Party.h"

#include<iostream>

class Arena{
	public:
		static Party* InitializePlayers(std::ostream * out);
		static Party* GenerateRivals(int rivalsCount);

	public:
		Arena(std::ostream* out, Party* players, Party* rivals, int minPartySize = 1, int maxPartySize = 2, bool dontMixPlayersAndRivals = false);
		~Arena();

		void Fight(int rounds);

	private:
		std::ostream* out;
		Party* pla;
		Party* riv;
		int min, max;
		bool mix;
};

