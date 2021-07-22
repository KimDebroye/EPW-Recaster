# EPW Recaster

![Overview](https://i.snipboard.io/i0SQku.jpg)

## Download
**[ [ Latest & Older Versions ](https://github.com/KimDebroye/EPW-Recaster/releases) ]**

___

## In a nutshell
EPW Recaster is a tool that
- automates recasting EPW weapons & gears (_armors_)
- using Optical Character Recognition and
- user-configurable search conditions.
> *EPW Recaster does not rely on nor uses any kind of game hook.<br />It solely relies on what is captured using OCR and performs<br />programmatical choices & actions based on captured results.*

___

## TL;DR QuickStart Video Demonstration/Guide

### Version 3.1
[![EPW Recaster ~ Demonstration Video](https://i.snipboard.io/iB6j5q.jpg)](https://youtu.be/i75cPTjQQ6Q)

___

## Setup

- Extract the contents of the provided package<br />to any folder that has write privileges.<br />( *f.e.* `Desktop` | `C:\Apps\EPW Recaster` | ... )
- Launch `EPW Recaster(.exe)`.
  - **[ Developer Note ]**<br />This tool may require admin privileges.<br />In case the OS configuration requires it<br />in order for the tool to run properly:
    - Right-click `EPW Recaster(.exe)`<br />and choose `Properties`.
    - In `Compatibility` tab,<br />check `☑ Run this program as an administrator`<br />and confirm by clicking `OK`.

___

## Sections

![Sections](https://i.snipboard.io/NvjpMd.jpg)

### General Notes
> - Once a preview or an auto-roll is started, the main form will be programmatically minimized and restored after.<br />( *The main form is mainly used for setup purposes only.* )
> - On the other hand, the info form will always stay on top of all windows.
> - All changes are automatically stored and restored upon relaunch.
> - Using any kind of text editor, theming options can be altered in<br />`.\Config\ThemeColorStyle.cfg` (*includes additional comments*).

___

### 1. ( Main ) Setup Form

___

#### 1-1. See-through Region

![See-through Region](https://snipboard.io/KETSjh.jpg)

- When launching EPW Recaster for the first time<br />( *and/or whenever the in-game location of the recast<br />a.k.a. reshape/manufacture window is changed* ),
- **move the tool around and resize using the size grip handle**<br />in order for:
  - the see-through region to fit the in-game recast<br />a.k.a. reshape/manufacture window,
  - the 3 tiny squares ( *hinting click regions* )<br />to be located somewhere over the in-game buttons<br />( `Retain the old attribute` |`Reproduce` | `Use the new attribute` ),
  - the capture region to fit the text to be captured.
    - **The fitting does not need to be pixel perfect in order for the Optical Character Recognition to work properly.**
    - Also, even though the following can ( *usually* ) be safely ignored due to implemented captured text filtering,<br />try to avoid including any additional UI elements in the captured region.<br />Reason: Depending on the fitting, parts of the in-game UI could be detected as a character<br />( *f.e. the in-game scroll up icon may be detected as capital 'A'* ).

___

#### 1-2. Capture Region

![Capture Region](https://snipboard.io/gimUN4.jpg)

- ( *A visible preview of* )
- The region setting the boundaries used for Optical Character Recognition.
- Depending in which mode the process will be started, the capture region will either be located:
  - **Preview Mode : full width of see-through region** and a little above the in-game buttons.
  - **Roll Mode : right half of see-through region** and a little above the in-game buttons.

> **❗ IMPORTANT NOTE ❗**<br />
> - **[ ! ] Without any actual game file alterations ( *`configs.pck`* ),<br />it is not recommended to use EPW Recaster<br />to look for stats on weapons that have unique (*long descriptive*) stats**,<br />unless it's (*one of those*) unique stats being targeted in a roll.
> - *In other words*, avoid looking for stats on weapons having<br />`Purify Spell`, `God of Frenzy`, `Square Formation`, `Soul Shatter`, `Spirit Blackhole`, ...<br />as a possible stat in order not to miss a stat needing an in-game scroll<br />(*unless the previously mentioned stats are being specifically targeted*).

___

#### 1-3. Condition List Switcher

![Condition List Switcher](https://i.snipboard.io/jYq52c.jpg)

- **Left Mouse Click**:
  - Select any of 5 condition list slots to work with.
- **Right Mouse Click**:
  - **Copy / Export Condition List**.
    - Can be used to:
      - share a condition list with anyone,
      - move a condition list to another slot by importing it.
      - back up a condition list ( *f.e. in a text document* ).
  - **Paste / Import Condition List**.
    - Can be used to:
      - import a condition list,
      - overwrite an existing condition list with another one.
  - **Clear Condition List**.
    - Clears all entries of a condition list.

___

#### 1-4. Condition List

![Condition List](https://i.snipboard.io/uOwQhv.jpg)

- A list containing preferred roll conditions.
- **Used in order to programmatically stop rolling when one of the listed required conditions is met**.
- The condition list can have both _fixed amount stats_ and _combo stats_ entries mixed.
- The order of entries can be changed by dragging an entry over to another location in the condition list.

___

#### 1-4-1. Fixed Amount Stats

![Fixed Amount Stats](https://i.snipboard.io/6YN19T.jpg)

- > **Although `REQUIRING A FIXED AMOUNT` of a preferred single or grouped stat,<br />rolled results `CAN HAVE ANY OTHER STAT`**.
- **Will accept a roll if**
  - an exact amount or more of a preferred single stat or of each of the grouped stats is detected.
- **Will reject a roll if**
  - an exact amount or more of a preferred single stat or of each of the grouped stats isn't detected.
- Recognizable by a blue stat color.
- Always preceded by a fixed minimum amount of a preferred stat.
- Can have up to 4 ( _grouped_ ) stat requirements per entry.
- Mainly used for rolls:
  - having equal stats:
    - _`4 x Interval Between Hits`_
  - needing a certain amount of stats:
    - _at least `2 x Channelling` ( and/or `any other stat rolled`_ )
    - ...

___

#### 1-4-2. Combo Stats

![Combo Stats](https://i.snipboard.io/2W7pMU.jpg)

- > **Although `NOT REQUIRING A FIXED AMOUNT` of a preferred single or grouped stat,<br />rolled results `CAN NOT HAVE ANY OTHER STAT`**.
- **Will accept a roll if**
  - a combination of at least one of each of the preferred grouped stats only is detected.
- **Will reject a roll if**
  - a combination of at least one of each of the preferred grouped stats isn't detected _or_
  - a stat is detected that isn't listed in the preferred grouped stats.
- Recognizable by a golden stat color.
- Are **not** preceded by a fixed minimum amount of a preferred stat.
- Can have up to 4 ( _grouped_ ) stat requirements per entry.
- Mainly used for rolls:
  - needing a uncertain amount of certain specific stats only:
    - _at least `1 x Channelling` & at least `1 x Reduce Physical Damage Taken` ( and NOT `any other stat rolled`_ )
    - ...

___

#### 1-4-3. Comparative Examples

| Condition | Would Accept | Would Reject |
| :--- | :--- | :--- |
| ![Fixed Amount Stats](https://i.snipboard.io/6YN19T.jpg) | ✅<br />*<sub><sup>• Channelling -3%<br/>• Channelling -2%<br/>• Channelling -3%<br />• Channelling -2%</sup></sub>*<hr />✅<br />*<sub><sup>• Channelling -3%<br />• Magic +9<br />• Channelling -2%<br />• Reduce Physical Damage Taken +2%</sup></sub>*<hr />✅<br />*<sub><sup>• Channelling -3%<br/>• Channelling -2%<br/>• Channelling -3%<br />• Magic +9</sup></sub>* | ❌<br />*<sub><sup>• Channelling -3%<br />• Magic +9<br/>• Magic +10<br />• Reduce Physical Damage Taken +2%</sup></sub>*<hr />❌<br />*<sub><sup>• Reduce Physical Damage Taken +2%<br />• Reduce Physical Damage Taken +1%<br/>• Reduce Physical Damage Taken +2%<br />• Reduce Physical Damage Taken +2%</sup></sub>*<hr />❌<br />*<sub><sup>• Channelling -3%<br />• Reduce Physical Damage Taken +1%<br/>• Reduce Physical Damage Taken +2%<br />• Reduce Physical Damage Taken +2%</sup></sub>* |
| | | |
| ![Combo Stats](https://i.snipboard.io/2W7pMU.jpg) | ✅<br />*<sub><sup>• Channelling -3%<br />• Reduce Physical Damage Taken +1%<br />• Channelling -2%<br />• Reduce Physical Damage Taken +2%</sup></sub>*<hr />✅<br />*<sub><sup>• Channelling -3%<br />• Channelling -2%<br />• Channelling -2%<br />• Reduce Physical Damage Taken +2%</sup></sub>*<hr />✅<br />*<sub><sup>• Reduce Physical Damage Taken +2%<br />• Reduce Physical Damage Taken +1%<br />• Reduce Physical Damage Taken +2%<br />• Channelling -3%</sup></sub>* | ❌<br />*<sub><sup>• Channelling -3%<br />• Channelling -2%<br/>• Channelling -3%<br />• Channelling -3%</sup></sub>*<hr />❌<br />*<sub><sup>• Channelling -3%<br />• Channelling -3%<br />• Magic +9<br/>• Channelling -2%</sup></sub>*<hr />❌<br />*<sub><sup>• Channelling -3%<br />• Channelling -3%<br />• Magic +9<br/>• Reduce Physical Damage Taken +2%</sup></sub>* |

___

#### 1-5. Condition Entry (Entries)

![Condition Entry](https://i.snipboard.io/BbaM3T.jpg)

> **❗ IMPORTANT NOTE ❗**<br />
> **Always put some thought in which rolled stats would be preferred and add all-encompassing conditions for those<br />in order not to miss out on any good rolls.**

- In order to enlist a roll condition:
  - Select a preferred amount and preferred stat to be found.
    - (Optional) Select up to 3 additional preferred amounts and preferred stats to be found/combined.
      - Once a second preferred stat has been selected from the drop-down list,<br />a checkbox to ignore amounts becomes available.<br />If checked, the entry would become a combo entry ( _allowing any amount of selected stats although limiting a roll to only contain the selected stats_ ).
  - Click the green `+` sign.
- Any previously added condition can be removed<br />by pressing the red `x` in the condition list.

> **Additional Notes**
>
>- **Ignore white stats, only blue stats are to be taken into account**.<br />
>  ( *f.e.* `4 x Phys. Res.` *= max, ignoring the fifth white Phys. Res. stat on a gear* ) 
>- When (*accidentally*) adding an amount larger than 1 of a unique stat ( *f.e. `Purify Spell`* ),<br />it will instead be enlisted as `1 x`.
>- When (*accidentally*) adding a summed amount exceeding the max stats possible,<br />it will instead be enlisted as either `4 x` or `5 x` ( _Atk. & Def. only_ ).
>- Using any kind of text editor, the list of selectable stat options can be altered in<br />`.\Config\Stats.cfg` (*includes additional comments*).

___

### 2. Info Form

___

#### 2-1. Form (Un)Chainer

![Form (Un)Chainer](https://i.snipboard.io/ANlV8a.jpg)

- **A toggle button attaching/detaching the info form to/from the main form.**
	- **Chained Mode** ( *attached forms mode | default at first launch* ) :
		- Only the main form will be movable and resizable.
		- Only the main form location and size will be stored and restored upon relaunch ( *due to the info form following its changes in location and/or size* ).
	- **Unchained Mode** ( *detached forms mode* )
		- Both main and info form will be separately movable and resizable.
		- Both form locations and sizes will be stored and restored upon relaunch.

___

#### 2-2. Log Folder

![Form (Un)Chainer](https://i.snipboard.io/NmaxXe.jpg)

- **Clicking this button opens the log folder.**
  - For each roll, a resulting text and image file is logged.
  - **[ ! ] Occasionally empty/delete this folder<br />in order to free up storage space**.

___

#### 2-3. OCR Result Info

![OCR Result Info](https://i.snipboard.io/tL4qcp.jpg)

- Displays text captured together with some additional info when previewing or rolling.

___

#### 2-4. Preview | Roll Mode

![Preview | Roll Mode](https://i.snipboard.io/69rtB2.jpg)

- **Preview Mode** ( *default at first launch* ) :
  - Once started, will perform one single text capture.
  - No rolls will be performed in-game.
- **Roll Mode**
  - Once started, will perform a set number of in-game rolls,
    - obeying any previously set conditions &
	- resulting in a programmatically moving mouse cursor and mouse clicks.
  - Can be stopped at any given time by clicking the `Stop` button.
  - Using any kind of text editor, timings can be altered in<br />  `.\Config\Params.cfg` (*includes additional comments*).

___

## FAQ

**<details><summary>` [ (Show|Hide Question|Answer ) "The tool doesn't seem to work for me ... what do I do, doc ?" ] `</summary>**

> **Question: "The tool doesn't seem to work for me ... what do I do, doc ?**"<br />
> **Symptoms**: "_No valid roll information detected (yet)._" | "_... doesn't seem necessary to roll any further ... halted ..._" | ...

➥ **Answer**:
- **In general, each capture/roll produces a logged text and image file that may be worth checking<br />in case it would be an OCR related issue.**<br />Check [ 2.2. Log Folder ](#2-2-log-folder) for more information.
- **It doesn't click/reproduce a roll.**
  - **It's most certainly an admin privilege issue.**<br />Check [ [ Setup > Developer Note ](#setup) ] for instructions on how to enable administrative privileges.
    - **[ Developer Note ]** This fixed it for most I've been chatting with that had this issue.<br />If many encounter this, I may include code in an update<br />to elevate administrative privileges programmatically ( _hoping it would skip the manual fix_ ).
  - **Additionally, make sure the capture region has been sized/positioned correctly.**
- **It does click/reproduce a roll but still stops a batch roll after a short while.**
  - **May as well be a timing issue.** On older or _trying-to-avoid-what-fries-and-chips-are-made-of-word_ computers,<br />maybe best to also increase timings ( _add about 500~xxxx milliseconds to timings of choice_ ).<br />Check [ [ 2-4. Preview | Roll Mode ](#2-4-preview--roll-mode) ] for the config file location.
  - Also, keep in mind that ( _at time of writing_ ) older re-rollable gears ( _Primal / Nirvana / ..._ )<br />don't produce in-game roll results as fast as R8 ones,<br />so also for this purpose it would be best to increase timings.<br />If needed, change the `Await In-Game Stats Rolled` timing to around 3750 milliseconds.
- **Inform me when the above does not provide a solution to the issue.**

</details>


**<details><summary>` [ (Show|Hide Question|Answer ) "Why did the tool skip a very exotic roll ?" ] `</summary>**

> **Question: "Why did the tool skip a very exotic roll ?**"

➥ **Answer**:
- **It did not meet any requirements set in the condition list.**<br />Always put some thought in which rolled stats would be preferred and add all-encompassing conditions for those.
- Another probability could be if the roll contained a unique stat with a long description.<br />Check [ [ 1.1. See-through Region ](#1-1-see-through-region) ] for more information.
- **Inform me with detailed information ( *and if possible steps to reproduce* ) if you think any of the above aren't the reason.<br />I would consider that a priority fix.**

</details>

**<details><summary>` [ (Show|Hide Question|Answer ) "Can I to contact you in any way / provide any feedback ?" ] `</summary>**

> **Question: "Can I to contact you in any way / provide any feedback ?**"

➥ **Answer**:
- **Sure.** Check below for ways to get in touch with me.<br />Feedback is always welcome and greatly appreciated.

</details>

___

## Contact | Feedback

- Post a message in the [ EPW Tool Release Info Thread ](https://epicpw.com/index.php?topic=68651.0).
- Feel free to post-message me in-game | on Discord.
