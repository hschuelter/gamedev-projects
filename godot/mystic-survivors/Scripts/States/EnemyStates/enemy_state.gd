extends State
class_name EnemyState

@export var enemy: Enemy
@onready var player: Player
var distance_to_player: float = 0

func Enter() -> void:
	pass

func Exit() -> void:
	pass

func Update(delta: float) -> void:
	pass

func Physics_Update(delta: float) -> void:
	pass
