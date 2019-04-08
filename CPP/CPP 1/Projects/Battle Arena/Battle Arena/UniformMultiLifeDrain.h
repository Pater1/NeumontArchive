#pragma once
#include "Attack.h"
class UniformMultiLifeDrain : public Attack{
	public:
		UniformMultiLifeDrain(double drainHit, double drainEfficiency);
		~UniformMultiLifeDrain();

	public:
		bool Throw(Character* self, std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out);

	private:
		double hit, efficiency;
};

