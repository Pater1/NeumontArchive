#include "Party.h"

#include "Cleric.h"
#include "Necromancer.h"
#include "Wizard.h"
#include "Barbarian.h"
#include "Rogue.h"

#include "RandUtils.h"
#include "NamesDatabase.h"

#include <cstdarg>
#include<iostream>

void Party::PartyPairAssembler(Party* partyA, Party* partyB, std::vector<Character*> players){
	int minPartySize = AllocatePlayersToParties(partyA, partyB, players);
	FillPartyToSize(partyA, partyB, minPartySize);
}

void Party::PartyPairAssembler(Party* partyA, Party* partyB, int minPartySize, std::vector<Character*> players){
	int minPrtySz = AllocatePlayersToParties(partyA, partyB, players);
	if (minPrtySz > minPartySize) minPartySize = minPrtySz;
	FillPartyToSize(partyA, partyB, minPartySize);
}

int Party::AllocatePlayersToParties(Party* partyA, Party* partyB, std::vector<Character*> players){
	for (int i = 0; i < players.size(); i++) {
		if (RandFloat() <= 0.5) {
			partyA->AddPartyMember(players[i]);
		} else {
			partyB->AddPartyMember(players[i]);
		}
	}

	int aSize = (int)partyA->GetPartyMembers()->size();
	int bSize = (int)partyB->GetPartyMembers()->size();

	return (aSize > bSize? aSize: bSize);
}

void Party::FillPartyToSize(Party* partyA, Party* partyB, int fillToSize){
	int aSize = (int)partyA->GetPartyMembers()->size();
	int bSize = (int)partyB->GetPartyMembers()->size();

	while (aSize < fillToSize) {
		partyA->AddPartyMember(partyA->WeightedRandomCharacter());
		aSize++;
	}
	while (bSize < fillToSize) {
		partyB->AddPartyMember(partyB->WeightedRandomCharacter());
		bSize++;
	}
}

Party::Party(){
	members = new std::vector<Character*>;
}

Party::Party(int PartySize){
	members = new std::vector<Character*>;
	while (members->size() < PartySize) {
		members->push_back(WeightedRandomCharacter(true));
	}
}

Party::Party(std::vector<Character*> PartyMembers) {
	members = &PartyMembers;
}

Party::Party(std::vector<Character*>* PartyMembers) {
	members = PartyMembers;
}

Party::~Party(){
	for (int i = 0; i < members->size(); i++) {
		delete (*members)[i];
	}
	delete members;
}

Character* Party::WeightedRandomCharacter(bool nonGenericName){
	int rogueOdds = (int)((barbarianCount <= 0 ? 10 : 5) * (totalCount <= 0 ? 1 : (rogueCount <= 0 ? 2 : 1-(double)totalCount/rogueCount)));
	int barbarianOdds = (int)((rogueCount <= 0 ? 10 : 5) * (totalCount <= 0 ? 1 : (barbarianCount <= 0 ? 2 : 1 - (double)totalCount/barbarianCount)));
	int clericOdds = (int)((necromancerCount <= 0? 10: 1) * (totalCount <= 0? 1: (clericCount <= 0? 2 : 1 - (double)totalCount/clericCount)));
	int necromancerOdds = (int)((clericCount <= 0 ? 10 : 1) * (totalCount <= 0 ? 1 : (necromancerCount <= 0 ? 2 : 1 - (double)totalCount/necromancerCount)));
	int wizardOdds = (int)((barbarianCount <= 0 ? 10 : 7) * (totalCount <= 0 ? 1 : (wizardCount <= 0 ? 2 : 1 - (double)totalCount/wizardCount)));

	int totalOdds = rogueOdds + barbarianOdds + clericOdds + necromancerOdds + wizardOdds;
	int roll = RandInt(totalOdds);
	int selector = 0;
	
	while (roll > 0) {
		roll -= (selector == 0 ? rogueOdds :
					(selector == 1 ? barbarianOdds :
						(selector == 2 ? clericOdds :
							(selector == 3 ? necromancerOdds :
								wizardOdds))));//only other option would be wizardOdds
		selector = (selector + 1) % 5;
	}

	Character* ret = nullptr;
	std::string name = (nonGenericName ? "" : NamesDatabase::AssembleRandomFullName());
	switch (selector) {
		case 0:
			ret = new Rogue(name);
			break;
		case 1:
			ret = new Barbarian(name);
			break;
		case 2:
			ret = new Cleric(name);
			break;
		case 3:
			ret = new Necromancer(name);
			break;
		case 4:
			ret = new Wizard(name);
			break;
	}

	return ret;
}

void Party::AddPartyMember(Character* cha){
	if (cha->className == "Cleric") {
		clericCount++;
	}else if (cha->className == "Rogue") {
		rogueCount++;
	}else if (cha->className == "Barbarian") {
		barbarianCount++;
	}else if (cha->className == "Necromancer") {
		necromancerCount++;
	}else if (cha->className == "Wizard") {
		wizardCount++;
	}

	totalCount++;
	members->push_back(cha);
}