#pragma once
#include "Attack.h"
class Hide : public Attack{
	public:
		Hide(double oddsToHide, std::string hideBlurb);
		~Hide();

	private:
		double hide, kill, fail;

	public:
		bool Hide::Throw(Character* self, std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out);
};

