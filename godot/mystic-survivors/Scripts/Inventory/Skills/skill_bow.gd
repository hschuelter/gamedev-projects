extends Skill
class_name SkillBow

func _init():
	super._init()
	bullet_scene = preload("res://Scenes/Projectiles/projectile_arrow.tscn")
	self.aim_strategy = AimPlayerInputStrategy.new()

func _set_weapon_data() -> void:
	self.weapon_data = preload("res://Resources/Weapons/Bow/bow_lv_0.tres")

func _set_weapon_scene() -> void:
	self.weapon_scene = preload("res://Scenes/Skills/skill_bow.tscn")
