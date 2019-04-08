#include "UniformMultiLifeDrain.h"



UniformMultiLifeDrain::UniformMultiLifeDrain(double drainHit, double drainEfficiency){
	hit = drainHit;
	efficiency = drainEfficiency;
	blurb = " drains life from the enemy party!\n";
}


UniformMultiLifeDrain::~UniformMultiLifeDrain(){
}

bool UniformMultiLifeDrain::Throw(Character* self, std::vector<Character*>* , std::vector<Character*>* targets, std::ostream* out) {
	if (out != NULL)
		*self->FullName(out) << blurb;

	double heal = 0;
	for (int i = 0; i < targets->size(); i++) {
		heal += (*targets)[i]->AlterHP(hit, out);
	}

	heal *= -efficiency;
	if (heal < 1) heal = 1;

	if (out == NULL) {
		self->AlterHP((int)heal, NULL);
	} else {
		self->AlterHP((int)heal, out);
	}
	return true;
}