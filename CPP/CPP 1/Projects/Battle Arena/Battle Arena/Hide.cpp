#include "Hide.h"
#include "RandUtils.h"

Hide::Hide(double oddsToHide, std::string hideBlurb){
	hide = oddsToHide;
	blurb = hideBlurb;
}


Hide::~Hide(){

}

bool Hide::Throw(Character* self, std::vector<Character*>* , std::vector<Character*>* , std::ostream* out) {
	if (self->hidden) {
		if (out != NULL)
			*self->FullName(out) << " remains" << blurb << "\n";
		
		return true;
	}
	
	self->hidden = RandFloat() <= hide;
	
	if (out != NULL) {
		if (!self->hidden) {
			*self->FullName(out) << " fails at" << blurb << "\n";
		}else{
			*self->FullName(out) << " succeeds in" << blurb << "\n";
		}
	}

	return true;
}