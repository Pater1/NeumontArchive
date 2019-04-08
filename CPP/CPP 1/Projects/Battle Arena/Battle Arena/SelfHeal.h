#pragma once
#include<vector>
#include "Attack.h"
class SelfHeal : public Attack{
	public:
		SelfHeal(double selfHeal);
		~SelfHeal();
	public:
		virtual bool Throw(Character* self, std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out = NULL);

	public:
		double heal;
};

