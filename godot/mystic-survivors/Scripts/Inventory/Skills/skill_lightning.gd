extends Skill
class_name SkillLightning

func _init():
	super._init()
	bullet_scene = preload("res://Scenes/Projectiles/projectile_lightning.tscn")
	self.aim_strategy = AimRandomEnemyStrategy.new()

func shoot(aim_vector : Vector2) -> void:
	var target: Vector2 = aim_vector
	
	var projectile: Projectile = bullet_scene.instantiate()
	projectile.global_position = target
	projectile._set_variables(
		_get_damage(), 
		_get_duration(), 
		_get_projectile_speed(), 
		_get_knockback(), 
		_get_pierce(),
		_get_size(),
		target
	)
	get_tree().root.add_child(projectile)

func _set_weapon_data() -> void:
	self.weapon_data = preload("res://Resources/Weapons/Lightning/lightning_lv_0.tres")

func _set_weapon_scene() -> void:
	self.weapon_scene = preload("res://Scenes/Skills/skill_lightning.tscn")
