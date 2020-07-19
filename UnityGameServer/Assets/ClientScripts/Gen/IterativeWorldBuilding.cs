using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IterativeWorldBuilding : MonoBehaviour
{
	private System.Random rnd = new System.Random();
	public Transform testEnv;
	public Material myMaterial = null;

	
	#region Flavor Lists
	/*	WORLD TYPES	*/
	string[] worldTypes = 
		{"Barren, with no atmosphere, no water, and scarce valuable resources; under a ceaseless barrage of terrible radiation.",
		"Barren, with a thin atmosphere and a distant sun; a cold, dead place, where a lonely wind blows neverending across flat planes of dark ice and stone.",
		"Barren, rich in mineral wealth and cooling after a long period of tectonic activity; massive thunderstorms lash this world, and methane ice-storms transform into world-sculpting glaciers.",
		"Barren, encased in ice that hides a deep, subglacial sea; the surface trembles with asteroid impacts and this world’s orbit spills ice and dust behind it.",
		"Barren, old, and close to its sun; the mountains have eroded to sand, and the dunes are endlessly white.",
		"Temperate and lush – forever hazy beneath a thick mist that all but blocks out the sun.",
		"Temperate and fertile, with myriad biomes and skies piled high with columnar clouds.",
		"Temperate and dry, marked by great swathes of plains and desert; vast alluvial deposits speak of the rivers that once were.",
		"Temperate; a world of archipelagos scattered across a mild sea.",
		"Temperate, with a healthy climate; this place is rich in native flora and fauna, and old, stable biomes.",
		"Inhospitable, with a thick and choking atmosphere; deep valleys hold pockets of breathable air.",
		"Inhospitable – a world of storm-tossed nitrogen oceans and wind-polished cadmium islands, where tides surge and recede for years and no land is safe from the flood or its retreat.",
		"Inhospitable, full of lava, and stalked by lightning; a new world, upon which there is no life to be found.",
		"Inhospitable, ever since a distant gamma-ray burst; where life once thrived, it is gone, and the land simmers with fires unceasing.",
		"Inhospitable, cracked open by an ancient impact; it bleeds heat into the vacuum as it slowly dies.",
		"A moon – temperate, with a stable atmosphere; the curve of its horizon visible from even a modest mountaintop. Its parent world looms massive above it, ever-present in its day and night sky.",
		"An icy moon, cold and dim – one of many around its parent world; asteroids that should pummel the larger planet are caught here instead.",
		"An ocean world; there is no land to be found.",
		"A dead world with a remnant atmosphere; massive geometric features and perfectly flat metal planes create an uncanny landscape of deep canals, carrying moaning winds the length of this world.",
		"An ancient world bathed in the light of its star, red and swollen in the sky; everything here has a terrible symmetry, as if nature itself had oriented – or been oriented – around a single landmark."};
	
	/*	DEFINING NATURAL FEATURES	*/
	string[] natFeats = 
		{"100-Year Storms - The storms come with increasing frequency and duration, typically in the form of massive hurricanes.",
		"Active Tectonics - Earthquakes take place more often here than on other worlds; on ocean worlds, this means a higher prevalence of tsunamis.",
		"Inert Core - This world’s core has stopped spinning, causing it to shed its magnetic field. Compasses do not work here, and UV exposure is dangerous without significant protection",
		"Monobiome - An oddity among worlds, this planet has a single, global biome and a single, global climate. It could be a world-spanning forest, desert, swamp, or something else entirely.",
		"Worldscar - Sometime in the last million years – a short time, geologically – this world suffered an impact from a massive stellar body. Its climate has leveled, but the scar remains a massive, visible feature.",
		"Royal Court - Hundreds of terrestrial moons dance in this planet’s orbit, ranging from hundreds of yards in diameter to thousands of kilometers. The night sky is bright, and the day is marked by gentle, dappled light.",
		"Under Twin Suns - This world orbits a binary star.",
		"Ringed - This world is banded by series of planetary rings, visible as a thin white line across the night sky. It’s an especially beautiful place to view from afar.",
		"Remote - Once a remote colonial, industrial, or scientific prospect, this world is far removed from human society and takes great effort to reach.",
		"Cosmopolitan - Interstellar shipping lanes pass near this world; it’s a frequent landmark, resupply point, or pit stop for interstellar travelers, who either remain in orbit or have an easy path down to the surface of the world.",
		"Hecatoncheires - This world is marked by a series of massive mountains – sheer peaks, rising kilometers into the sky.",
		"Epochal Sunset - This world is caught in the transition between geologic eras – ice to temperate, temperate to ice, and so on; expect a varied climate.",
		"Monument of Shame - Abandoned following massive climate destabilization, this world is now a dumping ground for passing ships and local corpro-states.",
		"Quarantine - Union red tape hides this world from the rest of the galaxy; the reasons may be known or unknown.",
		"Breathable Atmosphere - Perhaps surprisingly, this world has a breathable atmosphere; humans can breathe here without relying on an EVA, scrubbers, or other augmentation.",
		"High Gravity - This world has higher gravity than most; it ranges from 1–2 g greater than Cradle standard.",
		"Low Gravity - This world has lower gravity than most; it ranges from 0.1–0.99 g less than Cradle standard.",
		"Hard Sun - This world’s atmosphere provides little to no UV protection; venturing outside in the daylight is a risk without proper optical and dermal shielding.",
		"Dreamland - There is something otherworldly about this place – islands that float suspended in the air, oceans of liquid mercury, moving forests, or something stranger.",
		"Dust and Echoes - There are ancient, pre-Fall ruins on this world, with no indication as to their origin or nature."};
		
	/*	DEFINING ANTHROPOCENTRIC FEATURE	*/
	string[] anthroFeats = 
		{"Settlement: New - A new colonial settlement has recently been seeded here; drones and subalterns work tirelessly, clearing space and assembling the initial colony footprint. A small cluster of buildings house the landfall team – they are likely to welcome visitors, as they haven’t had any for years. The population numbers in the dozens.",
		"Settlement: First Generation - The colonial settlement here is young, with its first generation of native-born colonists now of-age and working to build a future home. There is a population of hundreds to thousands.",
		"Settlement: Stable - The colonial settlement here is in its second or third generation; stable, with a population in the tens of thousands.",
		"Outpost: Union Far-Field Team Mission - There is a far-field team outpost on this world, but no other human lives.",
		"Outpost: Relay Node - This world is empty save for a lone omninet relay node. A small team crews the node, providing security and on-site technical support; they rotate on a regular schedule.",
		"Outpost: SIGINT Station - The Union Navy maintains a small signals-intelligence (SIGINT) station here. There may or may not be a larger population, but even if there is, the SIGINT station stands somewhere high and remote.",
		"Outpost: Astrocartography Station - This world is orbited by one of Union’s astrocartography stations – a suite of automated telescopes, sensors, and observational equipment used to map distant stars and worlds.",
		"Outpost: Checkpoint/Forward Post - This world hosts a small Union garrison, likely made up of auxiliaries who report to a remote Union officer; their duty is to scout, stand watch, and await further orders. Most of their time is spent exercising, cleaning, maintaining gear, and waiting for something to happen.",
		"Installation: Research Facility - This world is the site of a research facility, belonging to Union, a corpro-state, or some other entity; it might be a secure, top-secret site, or it might not.",
		"Installation: Proving Ground - A significant part of this world – or maybe all of it – is given over to a proving ground, test field, ordnance firing range, or other large-scale installation for hazardous testing, training, measuring, or dumping. It is maintained by a long-term skeleton crew, although there is often a population surge during particular seasons or exercises.",
		"Installation: Deep Field Relay - This world features a secure, long-range sensor installation, probably located somewhere tall and remote. Populated by a small crew, the installation might be run by Union or a private entity.",
		"Installation: Union Embassy - Union’s embassies, like the one here, are modest buildings, usually found in the capital cities of Core worlds. If this world isn’t yet considered a Core world, it is likely to become one soon.",
		"Installation: Corpro-State Campus - Somewhere on this planet is the local campus of a corpro-state, home to a large permanent population. The campus might be an enclave, an exclave, or integrated into a larger urban environment, and is likely an administrative center with some level of public exposure or notoriety, whether it is publicly accessible or otherwise.",
		"Base: Naval Command - This world is home to the Union Navy’s regional system command (SysComm) center. Equal parts operations hub, garrison, shipyard, recruitment center, and medical facility, this facility is a large military base with everything that entails – defensive perimeters, sunken launch pads, reinforced buildings, and bunkers. SysComm centers house massive permanent populations and garrisons numbering in the thousands; additionally, they are almost always supported by a complement of ships with – at minimum – global strike and delivery capacity.",
		"Base: Capital - As the capital world for its home system, the physical heart of the system’s government is located here; these hubs might take the form of a campus, estate, block, or other large collection of hardened buildings, within which the state administration performs its duties. In more liberal states, there is likely to be some level of public access to this facility.",
		"Base: Uplift - There is a spaceport here – a sprawling launch facility open to the public, although restrictions on access are subject to local laws.",
		"Civic: Municipalities - Hundreds of millions of people live here, in modest towns and cities scattered across diverse biomes. There might be one or two signature cities, but there are vast stretches of wilderness between them and each municipality has a distinct culture and identity.",
		"Civic: Arcologies - The people of this world live in one or more arcologies – unified, self-contained ecosystem – cities, designed to exist in harmony with exterior and interior worlds. The residents of these arcologies may be strictly monitored and walled off from the larger world, or free to travel. This world is likely well-developed, with a history that reaches back at least several centuries; its global population numbers from hundreds of millions to single-digit billions.",
		"Civic: Metroswathes - Unlike arcology and municipal worlds, this world is intensively developed, with a significant proportion of landmass (<10–15%) given over to a metroswathe: a single, amalgamated urban environment that is home to billions. That such metropoles can function – and, likely, much of it does not – is miraculous. The formation and metastasis of metroswathes usually aren’t planned, although large sections are likely to have been developed intentionally. Vast and baroque criminal, bureaucratic, and community organizations work through avenues legal and otherwise, and organize extensively at street level.",
		"Union: Administrator’s Residence - Whether or not this was always the case, this world is habitable and has a stable population of millions. A Union administrator makes their residence here and, as such, the world has fast become the star system’s most important strategic hub."};
			
	/*	ENVIRONMENTS	*/
	string[] envA = 
		{"Dangerous Flora or Fauna - An unusually large proportion of this planet’s animal or plant life is dangerous; some of the flora and fauna may predatory, particularly hostile, or even titanic in size. Use the Monstrosity NPC type to generate encounters with wildlife. Hostile flora can appear on the battlefield as immobile characters with Size 1–2, 5 HP, and Evasion 10; targets that move adjacent to them must succeed on a Hull save or take 3􀀦 and become Immobilized until the flora is destroyed, as it traps them with sticky sap, webbing, a pit, or the like.",
		"Extreme Cold - Local cultures have adapted to the frozen climate, but mechs and pilots quickly freeze without a nearby source of heat. Mechs that don’t move or Boost on their turn become Immobilized at the end of their turn. This lasts until they break free with a successful Hull save as a quick action. In addition, all mechs gain Resistance to􀀥.",
		"Extreme Heat - Society has retreated mostly underground to escape this world’s blistering atmosphere. All 􀀥 inflicted (to the user or others) by systems or weapons is increased by +1.",
		"Thin Atmosphere - All characters gain Resistance to 􀀤.",
		"Extreme Sun - Characters take 1d6􀀥 whenever they are aren’t in shade at the end of a turn. Each character may only take this 􀀥 damage once per round.",
		"Corrosive Atmosphere - The dense atmosphere of this world eats through armor. All weapons gain AP.",
		"Particulate Storms This planet is swept by brutal, scouring storms of sand, rock, or metal. During storms, mechs always have soft cover. Pilots that leave their mech take 1􀀦AP each turn they are outside.",
		"Electrical Storms - This planet is swept by unusually strong electrical storms. During storms, choose a character at random at the end of each round: they must succeed on an ENGINEERING save with +1 Difficulty per level of Size or be STUNNED until the end of their next turn by a bolt of lightning.",
		"Disruptive Storms - The storms on this planet are so highly charged that electronic systems can’t function. All tech actions, attacks, and SYSTEMS checks and saves receive +1 Difficulty.",
		"Dangerous Storms - Storms of fire, meteors, acid rain, ice, or other destructive particles sweep this planet. During storms, all characters take 2􀀣AP at the end of their turns unless they are adjacent to an object that grants hard cover.",
		"Ocean World - Less than five percent of this world’s surface rises above the ocean. Mechs sink to the bottom and move as though in difficult terrain unless they are flying or have an EVA Module. Mechs can walk (slowly) on the bottom and are usually able to function in extremely high-pressure environments.",
		"Earthquakes - This world is regularly rocked by earthquakes. During earthquakes, roll 1d6 at the end of each round: on 1, all mechs must succeed on a HULL save or be knocked Prone unless they are flying.",
		"Molten World - Parts of this world’s crust juts through the surface in showers and pools of liquid rock. When characters move into areas of molten rock or lava for the first time on their turn or start their turn there, they take 5􀀣AP and 3􀀥.",
		"Primordial World - This world is a bubbling soup of semi-organic mud and gases. Humans must use breathing apparatuses or sealed suits outside of their mechs to survive the toxic atmosphere, and boiling mud creates numerous areas of both difficult and dangerous terrain.",
		"Low Gravity - Mechs count as flying when they Boost but must land after they move. Characters never take damage from falling.",
		"High Gravity - Mechs cannot Boost and are Immobilized instead whenever they would be Slowed. Prone mechs can stand even when Immobilized by this gravity, but must pass a HULL check to do so.",
		"Tomb World - This world has extremely high levels of ambient radiation, possibly because of nuclear war, atmospheric degradation, or something more sinister. Outside of mechs, humans without environmental protection temporarily decrease their maximum HP by 1 per hour of exposure. If they reach 0 HP this way, they die. They can regain their maximum HP by performing a Full Repair in a safe environment.",
		"Spire World - Instead of a surface, this world is comprised of countless floating islands or spires, each held aloft in a gaseous substrate and suspended through magnetic force. Perhaps the crust was shattered by a superweapon or natural disaster. Most of the remaining landmass is disconnected, although some islands are large enough to hold cities. Navigation systems are almost useless here.",
		"Sinking World - The surface of this world is covered in fine sand or thick mud. Mechs that move 1 space or less during their turn are Slowed. Slowed mechs that move 1 space or less are Immobilized and start sinking, eventually becoming completely engulfed. This effect lasts until an affected mech (or one adjacent to it) succeeds on a Hull save as a full action.",
		"Holy World - This world is beautiful and lacks especially dangerous features, but the local population holds it sacrosanct. Damaging any natural object – rocks, trees, and pristine grasslands, for example – incurs the wrath of the residents."};
	#endregion
	
	public string worldType, naturalFeat, anthroFeat, envType;
	
	public void Gen(){
		int a = rnd.Next(0,20), b = rnd.Next(0,20), c = rnd.Next(0,20), d = rnd.Next(0,20);
		
		worldType = worldTypes[a];
		naturalFeat = natFeats[b];
		anthroFeat = anthroFeats[c];
		envType = envA[d];
		
		ApplyMatToTestEnv(a);
	}
	
	void ApplyMatToTestEnv(int ind)
	{
		switch(ind){	
			case 0:
				myMaterial = Resources.Load("Materials/barrenGray") as Material;
				break;
			case 1:
				myMaterial = Resources.Load("Materials/barrenGray")as Material;
				break;	
			case 2:
				myMaterial = Resources.Load("Materials/barrenGray")as Material;
				break;
			case 3:
				myMaterial = Resources.Load("Materials/blueDark")as Material;
				break;
			case 4:
				myMaterial = Resources.Load("Materials/desertBrown")as Material;
				break;
			case 5:
				myMaterial = Resources.Load("Materials/barrenGray")as Material;
				break;
			case 6:
				myMaterial = Resources.Load("Materials/barrenGray")as Material;
				break;	
			case 7:
				myMaterial = Resources.Load("Materials/desertBrown")as Material;
				break;			
			case 8:
				myMaterial = Resources.Load("Materials/blueSea")as Material;
				break;			
			case 9:
				myMaterial = Resources.Load("Materials/barrenGray")as Material;
				break;
			case 10:
				myMaterial = Resources.Load("Materials/barrenGray")as Material;
				break;
			case 11:
				myMaterial = Resources.Load("Materials/blueDark")as Material;
				break;			
			case 12:
				myMaterial = Resources.Load("Materials/redSoil")as Material;
				break;			
			case 13:
				myMaterial = Resources.Load("Materials/barrenGray")as Material;
				break;			
			case 14:
				myMaterial = Resources.Load("Materials/barrenGray")as Material;
				break;
			case 15:
				myMaterial = Resources.Load("Materials/barrenGray")as Material;
				break;
			case 16:
				myMaterial = Resources.Load("Materials/moonWhite")as Material;
				break;	
			case 17:
				myMaterial = Resources.Load("Materials/moonBlue")as Material;
				break;
			case 18:
				myMaterial = Resources.Load("Materials/barrenGray")as Material;
				break;
			case 19:
				myMaterial = Resources.Load("Materials/redSoil")as Material;
				break;
			default:
				myMaterial = Resources.Load("Materials/barrenGray")as Material;
				break;
		}		
		
		Renderer[] allChildren = testEnv.GetComponentsInChildren<Renderer>();
		foreach (Renderer mesh in allChildren)
		{ 
			mesh.material = myMaterial;
		}
	}
	


}