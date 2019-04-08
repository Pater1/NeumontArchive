#pragma once
#include<string>

class DayTime{
	private:
		DayTime();

	public:
		DayTime(std::string parseIn);
		DayTime(std::string parseIn, std::ostream* stream);
		DayTime(int hr, int min, int sec);
		~DayTime();

	private:
		bool valid{ true };
		int hour{ 0 };
		int minute{ 0 };
		int second{ 0 };

	public:
		std::string Get24hrTime();
		std::string Get12hrTime();
		void AddTime(int hr, int min, int sec);
		inline bool IsValid() const { return valid; }

		void PrintTimes(std::ostream* stream);
};

