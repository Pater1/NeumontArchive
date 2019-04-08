#include "DayTime.h"
#include <sstream>
#include <iostream>
#include <string>
#include <regex>

namespace {
	void Parse24(DayTime* day, std::smatch parse) {
		int hr = std::stoi(parse[1]);
		int min = std::stoi(parse[2]);
		int sec = std::stoi(parse[3]);
		day->AddTime(hr, min, sec);
	}
	void Parse12(DayTime* day, std::smatch parse) {
		Parse24(day, parse);

		std::regex pmReg("[pP][mM]");
		if (regex_match(parse[4].str(), pmReg)) {
			day->AddTime(12, 0, 0);
		}
	}
}

DayTime::DayTime(std::string parseIn) : DayTime() {
	std::regex Hr24Regex ("([01]?[0-9]|2[0-4]):([0-5]?[0-9]):([0-5]?[0-9])");
	std::regex Hr12Regex ("(0?[0-9]|1[0-2]):([0-5]?[0-9]):([0-5]?[0-9]) ([pPaA][mM])");

	std::smatch match;
	
	valid = true;
	if (regex_match(parseIn, Hr24Regex)) {
		std::regex_search(parseIn, match, Hr24Regex);
		Parse24(this, match);
	} else if(regex_match(parseIn, Hr12Regex)) {
		std::regex_search(parseIn, match, Hr12Regex);
		Parse12(this, match);
	} else {
		valid = false;
	}
}
DayTime::DayTime(std::string parseIn, std::ostream* stream) : DayTime(parseIn) {
	if (stream != NULL && stream != nullptr) {
		*stream << "Parsing time for " << parseIn;
	}
}
DayTime::DayTime(){
	hour = 0;
	minute = 0;
	second = 0;
}

DayTime::DayTime(int hr, int min, int sec){
	AddTime(hr, min, sec);
}

DayTime::~DayTime(){}

std::string DayTime::Get24hrTime(){
	if (!valid) return "Invalid Time";

	std::ostringstream tim;
	if (hour < 10) {
		tim << "0";
	} tim << hour;

	tim << ":";

	if (minute < 10) {
		tim << "0";
	} tim << minute;

	tim << ":";

	if (second < 10) {
		tim << "0";
	} tim << second;

	return tim.str();
}

std::string DayTime::Get12hrTime(){
	if (!valid) return "Invalid Time";

	std::ostringstream tim;
	if (hour < 10) {
		tim << "0" << hour;
	} else {
		if (hour > 12) {
			if (hour - 12 < 10) {
				tim << "0";
			}
			tim << (hour - 12);
		} else {
			tim << hour;
		}
	}

	tim << ":";

	if (minute < 10) {
		tim << "0";
	} tim << minute;

	tim << ":";

	if (second < 10) {
		tim << "0";
	} tim << second;

	tim << " " << ((hour > 12) ? "PM" : "AM");

	return tim.str();
}

void DayTime::AddTime(int hr, int min, int sec){
	if (!valid) return;

	hour += hr;
	minute += min;
	second += sec;

	minute = minute + (second/60);
	second %= 60;

	hour = hour + (minute / 60);
	minute %= 60;

	hour %= 24;
}

void DayTime::PrintTimes(std::ostream* stream){
	if (stream != NULL && stream != nullptr) {
		if (valid) {
			*stream << Get12hrTime() << " in 24hr time is: " << Get24hrTime();
		} else {
			*stream << "Time input is invalid!";
		}
	}
}
