#include <GameEngine.h>

class Sandbox : public GameEngine::App {
public:
	Sandbox() {
		// Constructor
	}

	~Sandbox() {
		// Destructor
	}
};

GameEngine::App* GameEngine::CreateApp() {
	return new Sandbox();
}