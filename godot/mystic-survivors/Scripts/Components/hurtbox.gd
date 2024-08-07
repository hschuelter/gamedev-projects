extends Area2D

func enable_hurtbox() -> void:
	set_deferred("monitorable", true) 
	set_deferred("monitoring", true) 

func disable_hurtbox() -> void:
	set_deferred("monitorable", false) 
	set_deferred("monitoring", false)

signal take_damage
