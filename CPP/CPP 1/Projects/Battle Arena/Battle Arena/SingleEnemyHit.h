#pragma once
#include<vector>
#include "Attack.h"
class SingleEnemyHit : public Attack{
public:
	SingleEnemyHit(double damage, std::string hitBlurb, double oddsToStayHidden_OnKill = 0, double oddsToStayHidden_OnKillFail = 0);
	~SingleEnemyHit();

public:
	virtual bool Throw(Character* self, std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out = NULL);

private:
	double hitDamage, hideKill, hideFail;
};

