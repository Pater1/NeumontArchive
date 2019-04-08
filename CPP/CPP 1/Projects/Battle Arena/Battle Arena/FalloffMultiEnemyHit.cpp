#include "FalloffMultiEnemyHit.h"
#include "RandUtils.h"

FalloffMultiEnemyHit::FalloffMultiEnemyHit(double hitDamage, double falloffRate, std::string attackBlurb){
	hit = hitDamage;
	falloff = falloffRate;
	blurb = attackBlurb;
}


FalloffMultiEnemyHit::~FalloffMultiEnemyHit(){}

bool FalloffMultiEnemyHit::Throw(Character* self, std::vector<Character*>* , std::vector<Character*>* targets, std::ostream* out) {
	if (out != NULL)
		*self->FullName(out) << blurb << "\n";

	int startAt = RandInt((int)targets->size());
	int fallDivisor = -1;
	for (int i = 0; i < targets->size(); i++) {
		int index = (startAt + i) % targets->size();
		if ((*targets)[index] == NULL) continue;
		(*targets)[index]->AlterHP(hit * fallDivisor, out);
		fallDivisor = (int)(fallDivisor * falloff);
	}

	return true;
}
