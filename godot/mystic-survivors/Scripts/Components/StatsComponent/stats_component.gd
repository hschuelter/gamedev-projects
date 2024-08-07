extends Node
class_name StatsComponent

var base_stats: StatsPlayer

var health_modifier: float = 1.0

var move_speed_modifier: float = 1.0
var damage_modifier: float = 1.0
var duration_modifier: float = 1.0
var knockback_modifier: float = 1.0
var pierce_modifier: int = 0
var projectile_speed_modifier: float = 1.0
var cdr_modifier: float = 1.0 
var size_modifier: float = 1.0
var regen_modifier: float = 1.0
var magnet_modifier: float = 1.0
var amount_modifier: float = 1.0

func get_health() -> float:
	return base_stats.health * health_modifier
	
func get_move_speed() -> float:
	return base_stats.move_speed * move_speed_modifier

func get_damage() -> float:
	return base_stats.damage * damage_modifier
	
func get_duration() -> float:
	return base_stats.duration * duration_modifier
	
func get_knockback() -> float:
	return base_stats.knockback * knockback_modifier
	
func get_pierce() -> int:
	return base_stats.pierce + pierce_modifier
	
func get_projectile_speed() -> float:
	return base_stats.projectile_speed * projectile_speed_modifier
	
func get_cdr() -> float:
	return base_stats.cdr * cdr_modifier

func get_size() -> float:
	return base_stats.size * size_modifier

func get_regen() -> float:
	return base_stats.regen * regen_modifier

func get_magnet() -> float:
	return base_stats.magnet * magnet_modifier

func get_amount() -> int:
	return base_stats.amount * amount_modifier
