#pragma once
#include "Character.h"

class Party{
	public:
		static void PartyPairAssembler(Party* partyA, Party* partyB, std::vector<Character*> players);
		static void PartyPairAssembler(Party* partyA, Party* partyB, int minPartySize, std::vector<Character*> players);
	private:
		static int AllocatePlayersToParties(Party* partyA, Party* partyB, std::vector<Character*> players);
		static void FillPartyToSize(Party* partyA, Party* partyB, int fillToSize);

	public:
		Party();
		Party(int PartySize);
		Party(std::vector<Character*> PartyMembers);
		Party(std::vector<Character*>* PartyMembers);
		~Party();

	private:
		std::vector<Character*>* members;

	private:
		int totalCount{ 0 };
		int rogueCount{ 0 };
		int barbarianCount{ 0 };
		int clericCount{ 0 };
		int necromancerCount{ 0 };
		int wizardCount{ 0 };

	private:
		Character* WeightedRandomCharacter(bool nonGenericName = false);

	public:
		void AddPartyMember(Character* cha);
		std::vector<Character*>* GetPartyMembers() { return members; }
};

