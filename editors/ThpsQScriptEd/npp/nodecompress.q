LightIngore012 = { IgnoredLights = [ 0 1 2 ] }
LightIngore = { IgnoredLights = [ -1 0 ] }
LightIngoreAll = { IgnoredLights = [ -1 0 1 2 ] }



TrickInit = {
  TrickObject
  CreatedAtStart
}

Env = { NodeClass = EnvironmentObject }

OfflineEnv = {
  Env
  AbsentInNetGame
}

TrickEnv = {
  Env
  TrickObject
}

EnvInit = {
  Env
  CreatedAtStart
}

TrickEnvInit = {
  Env
  TrickInit 
}


TrickRailMetalInit = {
  RailMetal
  TrickInit 
}

TrickRailMetal = {
  RailMetal
  TrickObject 
}

RailMetalInit = {
  RailMetal
  CreatedAtStart
}

RailMetal = {
  NodeClass = RailNode 
  RailType = Metal 
  TerrainType = TERRAIN_METALSMOOTH 
}


TrickRailWoodInit = {
  RailWood
  TrickInit 
}

TrickRailWood = {
  RailWood
  TrickObject 
}

RailWoodInit = {
  RailWood
  CreatedAtStart
}

RailWood = {
  NodeClass = RailNode 
  RailType = Wood 
  TerrainType = TERRAIN_WOOD
}




TrickRailConcInit = {
  RailConc
  TrickInit 
}

TrickRailConc = {
  RailConc
  TrickObject 
}

RailConcdInit = {
  RailConc
  CreatedAtStart
}

RailConc = {
  NodeClass = RailNode 
  RailType = Concrete 
  TerrainType = TERRAIN_CONCSMOOTH
}


// LA trashcan

default_bouncy_params = {
	UpMagnitude = 20
	Bounciness = 1.5 
	MinBounceVel = 1.0
	BounceRot = 15
	ConstRot = 150
	Gravity = 90
	MinInitialVel = 10
}

Object_Bouncy = {
  Class = BouncyObject
	type = Barrier
	default_bouncy_params
	IgnoredLights = [ 0 1 ]
	AbsentInNetGames
}

LA_TrashCan = {
	Object_Bouncy
	model = "garbage"
	HitSound = BonkMetal
}



LOD01DIST = 500
LOD02DIST = 2000
LOD03DIST = 5000

//LA cars

default_dist_params = {
	lod_dist1 = LOD01DIST
	lod_dist2 = LOD02DIST
	lod_dist3 = LOD03DIST
}

LA_Car_Params = {
	Class = vehicle 
	default_dist_params
	IgnoredLights = [ 0 1 ] 
	AbsentInNetGames 
}
	
LA_Car_Gold = {
  LA_Car_params
	type = Gold 
	model = "buick_gold"
	model_lod1 = "buick_gold_LOD01"
	model_lod2 = "buick_gold_LOD02"
}
	
LA_Car_Purpl = {
  LA_Car_params
	type = Purpl
	model = "pickup_purpl"
	model_lod1 = "pickup_purpl_LOD01"
	model_lod2 = "pickup_purpl_LOD02"
}

LA_Car_Beige = {
  LA_Car_params
	type = Beige
	model = "oldsbeige"
	model_lod1 = "oldsbeige_LOD01"
	model_lod2 = "oldsbeige_LOD02"
}


LA_Car_Police = {
	LA_Car_params
	type = Police
	model = "police_car"
	model_lod1 = "police_car_LOD01"
	model_lod2 = "police_car_LOD02"
}


LA_Car_Lowrider = {
	LA_Car_params
	type = Low_Rider
	model = "low_rider"
	model_lod1 = "low_rider_LOD01"
	model_lod2 = "low_rider_LOD02"
}
