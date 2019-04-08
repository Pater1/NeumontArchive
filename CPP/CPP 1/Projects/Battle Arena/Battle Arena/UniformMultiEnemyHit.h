#pragma once
#include "Attack.h"
class UniformMultiEnemyHit : public Attack{
	public:
		UniformMultiEnemyHit(double damage, std::string hitBlurb);
		~UniformMultiEnemyHit();

	public:
		virtual bool Throw(Character* self, std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out = NULL);

	public:
		double hitDamage;
};

