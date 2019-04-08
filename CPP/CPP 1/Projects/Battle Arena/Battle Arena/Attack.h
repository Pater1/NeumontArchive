#pragma once

#include<vector>
#include<string>

#include "Character.h"

class Attack {
	public:
		virtual bool Throw(Character* self, std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out = NULL) = 0;

	public:
		std::string blurb;
};