extends Area2D

var knockback: float = 1

func enable_hitbox() -> void:
	set_deferred("monitorable", true) 
	set_deferred("monitoring", true) 

func disable_hitbox() -> void:
	set_deferred("monitorable", false) 
	set_deferred("monitoring", false) 
