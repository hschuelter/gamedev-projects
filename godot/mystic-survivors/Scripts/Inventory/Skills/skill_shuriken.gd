extends Skill
class_name SkillShuriken

func _init():
	super._init()
	bullet_scene = preload("res://Scenes/Projectiles/projectile_shuriken.tscn")
	self.aim_strategy = AimNearestEnemyStrategy.new()

func rotate_to_direction(vector: Vector2) -> void:
	pass

func _set_weapon_data() -> void:
	self.weapon_data = preload("res://Resources/Weapons/Shuriken/shuriken_lv_0.tres")

func _set_weapon_scene() -> void:
	self.weapon_scene = preload("res://Scenes/Skills/skill_bow.tscn")
