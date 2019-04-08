#pragma once
#include<iostream>
class Currency{
	public:
		Currency();
		Currency(int totalCoppers);
		Currency(int copper, int silver, int gold, int mythril);
		~Currency();

	private:
		const int ccToSc = 100;
		const int scToGc = 100;
		const int gcToMb = 25;

		const float ccToLb = 10 / 100;
		const float scToLb = 8.0f / 100;
		const float gcToLb = 5.0f / 100;
		const float mbToLb = 1;

		int cop{ 0 };
		int silv{ 0 };
		int gld{ 0 };
		int myth{ 0 };

		void SyncFromTotal(int total);

	public:
		int GetTotal();
		int GetTotal(int copper, int silver, int gold, int mythril);

		const int& _Copper() const { return cop; }
		const int& _Silver() const { return silv; }
		const int& _Gold() const { return gld; }
		const int& _Mythril() const { return myth; }

		void AlterByTotal(int totalDelta);
		void AlterByCoinage(int copperDelta, int silverDelta, int goldDelta, int mythrilDelta);

		std::ostream* PrintMoney(std::ostream* out);
};

