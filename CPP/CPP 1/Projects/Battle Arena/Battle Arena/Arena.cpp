#include "Arena.h"
#include "Party.h"
#include "Battle.h"

#include "DeleteProtectionWrapper.h"

#include"LoginUtils.h"

Party* Arena::InitializePlayers(std::ostream* out){
	Character* c = LoginUtils::LoginAndAssembleCharacter(out);
	c->FullName(&std::cout);
	Party* p = new Party();
	p->AddPartyMember(c);
	return p;
}

Party* Arena::GenerateRivals(int rivalsCount){
	return new Party(rivalsCount);
}

Arena::Arena(std::ostream* outStream, Party* players, Party* rivals, int minPartySize, int maxPartySize, bool dontMixPlayersAndRivals){
	out = outStream;
	pla = players;
	riv = rivals;
	min = minPartySize;
	max = maxPartySize;
	mix = dontMixPlayersAndRivals;

	for (int i = 0; i < players->GetPartyMembers()->size(); i++) {
		(*players->GetPartyMembers())[i]->cachedOut = outStream;
	}
	for (int i = 0; i < rivals->GetPartyMembers()->size(); i++) {
		(*rivals->GetPartyMembers())[i]->cachedOut = outStream;
	}
}


Arena::~Arena(){
	delete pla;
	delete riv;
}

void Arena::Fight(int rounds){
	for (int i = 0; i < rounds; i++) {
		std::vector<Character*> AB;
		for (int j = 0; j < pla->GetPartyMembers()->size(); j++) {
			AB.push_back(new DeleteProtectionWrapper(
				(*pla->GetPartyMembers())[j]
			));
		}
		for (int j = 0; j < riv->GetPartyMembers()->size(); j++) {
			AB.push_back(new DeleteProtectionWrapper(
				(*riv->GetPartyMembers())[j]
			));
		}

		Party* partyA = new Party();
		Party* partyB = new Party();
		Party::PartyPairAssembler(partyA, partyB, 2, AB);

		Battle* b = new Battle(partyA, partyB, out);
		b->AutoBattle(out);
		delete b;//also deletes parties
	}
}