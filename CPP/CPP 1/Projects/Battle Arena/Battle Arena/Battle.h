#pragma once
#include "Character.h"
#include "Party.h"

class Battle{
	public:
		Battle(Party* partyA, Party* partyB, std::ostream* out = NULL);
		Battle(std::vector<Character*>* partyA, std::vector<Character*>* partyB, std::ostream* out = NULL);
		~Battle();

	public:
		bool Turn(std::ostream* out = NULL);
		void AutoBattle(std::ostream* out = NULL);

	private:
		std::vector<Character*>* A;
		std::vector<Character*>* B;
		bool refsParties{ false };
		Party* pA;
		Party* pB;
		int turnCount{ 0 };
		std::ostream* cachedOut;

		void DisperseAwards();
};