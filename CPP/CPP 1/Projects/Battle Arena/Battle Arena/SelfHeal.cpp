#include "SelfHeal.h"

#include<vector>


SelfHeal::SelfHeal(double selfHeal){
	heal = selfHeal;
	blurb = " heals themself. \n";
}


SelfHeal::~SelfHeal(){
}

bool SelfHeal::Throw(Character* self, std::vector<Character*>* , std::vector<Character*>* , std::ostream* out) {
	if(out != NULL)
		*self->FullName(out) << blurb;

	self->AlterHP(heal, out);
	return true;
}