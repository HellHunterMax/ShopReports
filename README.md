# ShopReports
A command line program that reads Files with transactions with different extensions and creates reports for them. Command must be given in the args when the program runs as following:
`programname "path/to/input" "command" "path/to/output"`. The "command" format will be specified below (`in parantheses`).

## Can read/write the folowing file types:
- .json
- .xml
To be implemented
- .csv

### write info.
the program will write the same file type as read from.

### read info:
transactions must contain the following info:
- ShopName
- City
- Street
- Item
- DateTime
- Price

## possible reports:
1) By time (`time`)
    - how many items have been bought during every hour of time of day,
    - how much money did every hour total (on average), 
    - and get rush hour (most mony earned on average per day).
    - selected range of hours as well. "time 11:00-17:00"

2) What city earned the most/least money and what city sold the most/least items? (`city [-min/-max] [-items/-money]`)

3) Daily money earned for specific shop. (`daily Shop Name`)
 - ascending order (`daily Shop Name -asc`)
 - descending order (`daily Shop Name -desc`)

4) What items were sold in what shop, at what price and when (file, named after shop).  (`full`)
 - no output file path needed.
 - ascending order (`full -asc`)
 - descending order (`full -desc`)

