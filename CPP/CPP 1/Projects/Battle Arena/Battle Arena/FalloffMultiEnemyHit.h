#pragma once
#include "Attack.h"
class FalloffMultiEnemyHit : public Attack{
	public:
		FalloffMultiEnemyHit(double hitDamage, double falloffRate, std::string attackBlurb);
		~FalloffMultiEnemyHit();

	private:
		double hit, falloff;

	public:
		bool FalloffMultiEnemyHit::Throw(Character* self, std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out);
};

