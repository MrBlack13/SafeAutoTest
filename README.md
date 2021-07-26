# SafeAutoTest
Safe Auto's test (it was a fun trip down memory lane)
So I haven't programmed in C# in a long time. Some of the quirks and problems/solutions I had are coming back to me.

My thought process was to create Arrays of Objects (pardon my lingo, I haven't spoke or thought C# in a long time).   What I mean by “Object” is a Struct, Class or Object thing that can hold a methods for internal calculations.

We need an Array (or IList) of Drivers and Drivers need an Array of Trips.
Drivers have properties we need to display. [DriverName, TotalMiles, AverageSpeed].
Trips need properties saved and calculated. [Miles, Minutes, TripMPH]
Each Driver would need to hold their own Trips and and calculate values from that.

Right of the get go, I saw that our input was a sloppy command line string. Anyone can type any slop and the program would need to deal with that first.  So I started by developing syntax checking code.  As an after thought I probably could move some or possibly all of that code into the Driver or Trip Classes.  While checking command line syntax, I also convert strings into appropriate data types for processing.

One error I noticed that you didn't request but probably should is an Impossible Trip.  One person cannot take two trips that overlap. This error would need to go into the Drivers class to see if Trips overlapped timespans before adding the trip to the Driver's trip array.

I attempted to make some properties Private but ran into trouble.  In a real situation I would have figured that kind of thing out.  Like my getters and setters are kind of funky.
I would have liked to figure out (if it's even a good idea) to hold and reference Drive Class in an Array instead of using the IList.

For a large project with a team, I would leave it up to the team but I see that in C# you can put classes in separate file.  I am not sure if the team would think this is more confusing to not have it all in one place or it might be nicer to have smaller files to look through. Sometimes too many files can be a burden too.
