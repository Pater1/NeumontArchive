#include "UniformMultiEnemyHit.h"



UniformMultiEnemyHit::UniformMultiEnemyHit(double damage, std::string hitBlurb){
	hitDamage = damage;
	blurb = hitBlurb;
}


UniformMultiEnemyHit::~UniformMultiEnemyHit(){
}

bool UniformMultiEnemyHit::Throw(Character* self, std::vector<Character*>* , std::vector<Character*>* targets, std::ostream* out) {
	if (out != NULL)
		*self->FullName(out) << blurb;

	for (int i = 0; i < targets->size(); i++) {
		(*targets)[i]->AlterHP(-hitDamage, out);
	}
	return true;
}