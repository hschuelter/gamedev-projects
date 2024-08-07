extends EnemyState
class_name EnemyAttackState

@export var DISTANCE_TO_CHASE: float = 120
@onready var enemy_attack: EnemyAttack = $EnemyAttack
@onready var cooldown_timer = $CooldownTimer

func Enter() -> void:
	cooldown_timer.timeout.connect(_on_cooldown_timer_timeout)
	cooldown_timer.wait_time = enemy_attack.cooldown_duration
	cooldown_timer.start()

func Exit() -> void:
	cooldown_timer.timeout.disconnect(_on_cooldown_timer_timeout)
	cooldown_timer.stop()

func Update(delta: float) -> void:
	pass

func Physics_Update(delta: float) -> void:
	distance_to_player = enemy.global_position.distance_to(player.global_position)
	enemy.velocity = Vector2.ZERO
	if distance_to_player >= DISTANCE_TO_CHASE:
		transition.emit(self, "chase")

func _on_cooldown_timer_timeout():
	enemy_attack.Attack(player, enemy)
	#cooldown_timer.wait_time = cooldown_duration
	cooldown_timer.start()
