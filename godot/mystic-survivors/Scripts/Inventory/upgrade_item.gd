extends Upgrade
class_name UpgradeItem

func _init(passive_item: PassiveItem):
	self.passive_item = passive_item
	self.title = passive_item.title
	self.level = 0
	self.upgrade = passive_item
	self.icon = Sprite2D.new()
	self.icon.texture = upgrade.item_data.icon

func get_upgrade() -> PassiveItem:
	return self.upgrade

func get_description() -> String:
	return self.passive_item.description
