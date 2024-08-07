extends Skill
class_name SkillFireball

func _init():
	super._init()
	bullet_scene = preload("res://Scenes/Projectiles/projectile_fireball.tscn")
	self.aim_strategy = AimRandomStrategy.new()

func _set_weapon_data() -> void:
	self.weapon_data = preload("res://Resources/Weapons/Fireball/fireball_lv_0.tres")

func _set_weapon_scene() -> void:
	self.weapon_scene = preload("res://Scenes/Skills/skill_fireball.tscn")
