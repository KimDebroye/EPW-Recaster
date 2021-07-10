
# EPW Recaster

![Overview](https://snipboard.io/V7GhpE.jpg)

## In a nutshell
EPW Recaster is a tool that automates recasting EPW weapons & gears<br />using Optical Character Recognition and user-configurable search conditions.<br />
**[ Side note ]** *EPW Recaster does not rely on nor uses any kind of game hook.<br />It solely relies on what is captured using OCR and performs<br />programmatical choices & actions based on captured results.*

## Setup
- Extract the contents of the provided package<br />to any folder that has write privileges.<br />( *f.e.* `Desktop` | `C:\Apps\EPW Recaster` | ... )
- Launch `EPW Recaster(.exe)`.
	- **[ Developer Note ]**<br />This tool shouldn't require admin privileges by default.<br />However, if the OS configuration does require it<br />in order for the tool to run properly:
		- Right-click `EPW Recaster(.exe)`<br />and choose `Properties`.
		- In `Compatibility` tab,<br />check `☑ Run this program as an administrator`<br />and confirm by clicking `OK`.

<div style="page-break-after: always"></div>

## Sections

![Sections](https://snipboard.io/KNDg30.jpg)

### General Notes
- Once a preview or an auto-roll is started, the main form will be programmatically minimized and restored after. It is mainly used for setup purposes only.
- On the other hand, the info form will always stay on top of all windows.
- All changes are automatically stored and restored upon relaunch.

<div style="page-break-after: always"></div>

### 1. ( Main ) Setup Form

#### 1-1. See-through Region
- When launching EPW Recaster for the first time
- ( *and/or whenever the in-game location of the recast<br />a.k.a. reshape/manufacture window is changed* ),
- **move the tool around and resize using the size grip handle**
- in order for:
	- the see-through region to fit the in-game recast<br />a.k.a. reshape/manufacture window,
	- the 3 tiny squares ( *hinting click regions* )<br />to be located somewhere over the in-game buttons<br />( `Retain the old attribute` |`Reproduce` | `Use the new attribute` ),
	- the capture region to fit the text to be captured.

> Additional Notes

- **The fitting does not need to be pixel perfect in order for the Optical Character Recognition to work properly.**
- Depending on the fitting, parts of the in-game UI could be detected as a character<br />( *f.e. the in-game scroll up icon may be detected as capital 'A'* ).<br />This can be safely ignored.
- **[ ! ] It is strongly discouraged to use EPW Recaster to look for stats on weapons that have unique (*long descriptive*) stats**, unless it's (*one of those*) unique stats being sought after.<br />*In other words*, avoid looking for stats on weapons having `Purify Spell`, `God of Frenzy`, `Square Formation`, `Soul Shatter`, `Spirit Blackhole`, ... as a possible stat in order not to miss a stat needing an in-game scroll (*unless the previously mentioned stats are being sought after*).

#### 1-2. Capture Region
- ( *A visible preview of* )
- The region setting the boundaries used for Optical Character Recognition.
- Depending in which mode the process will be started, the capture region will either be located:
	- **Preview Mode : full width of see-through region** and a little above the in-game buttons.
	- **Roll Mode : right half of see-through region** and a little above the in-game buttons.

#### 1-3. Condition List
- A list containing previously added roll conditions
- **used in order to programmatically stop rolling when one of the listed conditions is met**.
	- *In other words*, the list needs to be read as *f.e.*<br />*Stop rolling when* `2 x Channelling` **OR** `2 x Reduce Magic Damage Taken` *is found*.
- Any previously added condition can be removed by pressing the red `x`.

#### 1-4. Condition Entry
- In order to enlist a roll condition:
	1. Select the amount of the preferred stat to be found.
	2. Select the preferred stat to be found.
	3. Click the green `+` sign.

> Additional Notes

- **[ ! ] When looking for f.e. `Phys. Res.` | `Range` | `...`,<br />take into account that often white stats already contain these stats and thus<br />will increase the hit rate counter by 1 by default.**<br />*In other words*, when looking for f.e. `4 x Phys. Res.` (*blue stats*),<br />the amount condition needs to be set to<br />`4 x` *`(blue stats)`* `Phys. Res.` + `1 x` *`(default white stat)`* `Phys. Res.` =<br />**`5 x`** instead.
- When (*accidentally*) adding an amount larger than 1 of a unique stat ( *f.e. `Purify Spell`* ),<br />it will instead be enlisted as `1 x`.
- The HP stat has not been listed as an option.
- Using any kind of text editor, the list of selectable stat options can be altered in<br />`.\Config\Stats.cfg` (*includes additional comments*).

<div style="page-break-after: always"></div>

### 2. Info Form

#### 2-1. Form (Un)Chainer
- **A toggle button attaching/detaching the info form to/from the main form.**
	- **Chained Mode** ( *attached forms mode | default at first launch* ) :
		- Only the main form will be movable and resizable.
		- Only the main form location and size will be stored and restored upon relaunch ( *due to the info form following its changes in location and/or size* ).
	- **Unchained Mode** ( *detached forms mode* )
		- Both main and info form will be separately movable and resizable.
		- Both form locations and sizes will be stored and restored upon relaunch.

#### 2-2. OCR Result Info
- Displays all text captured together with some additional info when previewing or rolling.

#### 2-3. Preview | Roll Mode
- **Preview Mode** ( *default at first launch* ) :
	- Once started, will perform one single text capture.
	- No rolls will be performed in-game.
- **Roll Mode**
	- Once started, will perform a set number of in-game rolls,
		- obeying any previously set conditions &
		- resulting in a programmatically moving mouse cursor and mouse clicks.
	- Can be stopped at any given time by clicking the `Stop` button.
		- *For the time being ... ahum ... be quick enough in order to press it ;)*<br />( *A small pause is implemented inbetween rolls.* ) 
		- **[ Developer Note ]** If so desired, a low level keyboard hook to f.e. detect the *`Escape`* key in order to stop a running roll process can be implemented at any given time, however this would result in the application requiring admin privileges ( *which can be elevated programmatically if needed, although I tend to avoid such at an initial release* ).
