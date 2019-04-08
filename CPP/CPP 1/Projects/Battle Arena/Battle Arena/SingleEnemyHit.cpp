#include "SingleEnemyHit.h"
#include "RandUtils.h"
#include<vector>



SingleEnemyHit::SingleEnemyHit(double damage, std::string hitBlurb, double oddsToStayHidden_OnKill, double oddsToStayHidden_OnKillFail){
	hitDamage = damage;
	blurb = hitBlurb;
	hideKill = oddsToStayHidden_OnKill;
	hideFail = oddsToStayHidden_OnKillFail;
}


SingleEnemyHit::~SingleEnemyHit(){

}

bool SingleEnemyHit::Throw(Character* self, std::vector<Character*>* , std::vector<Character*>* targets, std::ostream* out) {
	Character* hit = nullptr;

	int selection = RandInt((int)targets->size());
	int wrapCheck = selection;
	do {
		if ((*targets)[selection] == NULL) continue;
		hit = (*targets)[selection];
		selection = (selection + 1) % targets->size();
		if (wrapCheck == selection) {
			if (out != nullptr)
				*self->FullName(out) << " has nothing to attack, and will do nothing this turn.";
			return false;
		}
	} while (hit->PeekDead());

	if (out != NULL) {
		*self->FullName(out) << blurb; *hit->FullName(out) << ".\n";
		if (self->hidden)
			*out << "	Sneak attack for double damage!\n";
	}

	int damage = hit->AlterHP(-hitDamage*(self->hidden? 2: 1), (out == NULL ? NULL : out));

	if (self->hidden) {
		double stayHiddenOdds = (damage >= hit->PeekHP()? hideKill: hideFail);
		self->hidden = RandFloat() < stayHiddenOdds;

		if (out != NULL) {
			if (self->hidden) {
				*out << "	"; *self->FullName(out) << " remains hidden.\n";
			} else {
				*out << "	"; *self->FullName(out) << " is exposed!\n";
			}
		}
	}

	return true;
}