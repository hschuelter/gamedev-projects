#pragma once

#include "Core.h"

// ge = GameEngine
namespace GameEngine {
	
	class GAMEENGINE_API App {
	
	public:
		App();
		virtual ~App();

		void Run();
	};

	// To be defined in client
	App* CreateApp();
}

