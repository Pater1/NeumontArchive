#include "SumonSimpleUndead.h"
#include "SimpleUndead.h"

#include <iostream>

SumonSimpleUndead::SumonSimpleUndead(){
	blurb = " summons ";
}


SumonSimpleUndead::~SumonSimpleUndead(){
}

bool SumonSimpleUndead::Throw(Character* self, std::vector<Character*>* allys, std::vector<Character*>* , std::ostream* out) {
	Character* hit = new SimpleUndead();

	allys->push_back(hit);


	if (out != NULL && out != nullptr) {
		*self->FullName(out) << blurb; *hit->FullName(out) << ".\n";
	}
	return true;
}
