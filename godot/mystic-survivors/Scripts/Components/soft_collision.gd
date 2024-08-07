extends Area2D

@onready var current_areas: Array = []

func is_colliding() -> bool:
	return current_areas.size() > 0

func get_push_vector() -> Vector2:
	var push_vector = Vector2.ZERO
	if current_areas.size() > 0:
		for i in range(current_areas.size()):
			var other = current_areas[i]
			push_vector += self.global_position - other.global_position
		push_vector = push_vector.normalized()
	return push_vector

func enable_soft_collision() -> void:
	set_deferred("monitorable", true) 
	set_deferred("monitoring", true) 

func disable_soft_collision() -> void:
	set_deferred("monitorable", false) 
	set_deferred("monitoring", false)

func _on_area_entered(area: Area2D) -> void:
	current_areas.push_back(area)

func _on_area_exited(area: Area2D) -> void:
	current_areas.erase(area)
