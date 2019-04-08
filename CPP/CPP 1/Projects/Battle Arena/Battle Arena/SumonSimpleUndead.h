#pragma once
#include "Attack.h"
class SumonSimpleUndead : public Attack{
	public:
		SumonSimpleUndead();
		~SumonSimpleUndead();
		bool Throw(Character * self, std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream * out);
};

