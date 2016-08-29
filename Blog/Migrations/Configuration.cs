namespace Blog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            if (!context.Users.Any())
            {
                // If the database is empty, populate sample data in it

                CreateUser(context,"Admin", "admin@gmail.com", "123", "System Administrator");
                CreateUser(context,"Pesho", "pesho@gmail.com", "123", "Peter Ivanov");
                CreateUser(context,"Meri", "merry@gmail.com", "123", "Maria Petrova");
                CreateUser(context, "george", "geshu@gmail.com", "123", "George Petrov");

                CreateRole(context, "Administrators");
                AddUserToRole(context, "Admin", "Administrators");

                CreatePost(context,
                    title: "Spiced carrot & lentil soup",
                    description: "A delicious, spicy blend, packed full of iron and low fat to boot. It's ready in under half an hour or can be made in a slow cooker",
                    body: @"Heat a large saucepan and dry-fry the cumin seeds and chilli flakes for 1 min, or until they start to jump around the pan and release their aromas. Scoop out about half of the seeds with a spoon and set aside. Add the oil, carrot, lentils, stock and milk to the pan and bring to the boil. Simmer for 15 mins until the lentils have swollen and softened.
Whizz the soup with a stick blender or in a food processor until smooth (or leave it chunky if you prefer). Season to taste and finish with a dollop of yogurt and a sprinkling of the reserved toasted spices. Serve with warmed naan breads.",
                    date: new DateTime(2016, 03, 27),
                    authorUsername: "Meri"
                    
                );

                CreatePost(context,
                    title: "Chilli con carne",
                    description: "This great chilli has to be one of the best dishes to serve to friends for a casual get-together",
                    body: @"Prepare your vegetables. Chop 1 large onion into small dice, about 5mm square. The easiest way to do this is to cut the onion in half from root to tip, peel it and slice each half into thick matchsticks lengthways, not quite cutting all the way to the root end so they are still held together. Slice across the matchsticks into neat dice. Cut 1 red pepper in half lengthways, remove stalk and wash the seeds away, then chop. Peel and finely chop 2 garlic cloves.
Start cooking. Put your pan on the hob over a medium heat. Add the oil and leave it for 1-2 minutes until hot (a little longer for an electric hob). Add the onions and cook, stirring fairly frequently, for about 5 minutes, or until the onions are soft, squidgy and slightly translucent. Tip in the garlic, red pepper, 1 heaped tsp hot chilli powder or 1 level tbsp mild chilli powder, 1 tsp paprika and 1 tsp ground cumin. Give it a good stir, then leave it to cook for another 5 minutes, stirring occasionally.
Brown the 500g lean minced beef. Turn the heat up a bit, add the meat to the pan and break it up with your spoon or spatula. The mix should sizzle a bit when you add the mince. Keep stirring and prodding for at least 5 minutes, until all the mince is in uniform, mince-sized lumps and there are no more pink bits. Make sure you keep the heat hot enough for the meat to fry and become brown, rather than just stew.
Making the sauce. Crumble 1 beef stock cube into 300ml hot water. Pour this into the pan with the mince mixture. Open 1 can of chopped tomatoes (400g can) and add these as well. Tip in ½ tsp dried marjoram and 1 tsp sugar, if using (see tip at the bottom), and add a good shake of salt and pepper. Squirt in about 2 tbsp tomato purée and stir the sauce well.
Simmer it gently. Bring the whole thing to the boil, give it a good stir and put a lid on the pan. Turn down the heat until it is gently bubbling and leave it for 20 minutes. You should check on the pan occasionally to stir it and make sure the sauce doesn’t catch on the bottom of the pan or isn’t drying out. If it is, add a couple of tablespoons of water and make sure that the heat really is low enough. After simmering gently, the saucy mince mixture should look thick, moist and juicy.
Bring on the beans. Drain and rinse 1 can of red kidney beans (410g can) in a sieve and stir them into the chilli pot. Bring to the boil again, and gently bubble without the lid for another 10 minutes, adding a little more water if it looks too dry. Taste a bit of the chilli and season. It will probably take a lot more seasoning than you think. Now replace the lid, turn off the heat and leave your chilli to stand for 10 minutes before serving, and relax. Leaving your chilli to stand is really important as it allows the flavours to mingle and the meat.
Serve with soured cream and plain boiled long grain rice.",
                    date: new DateTime(2016, 05, 11),
                    authorUsername: "Meri"
                );

                CreatePost(context,
                    title: "Best-ever brownies",
                    description: "A foolproof recipe for squidgy cake squares, studded with extra chunks of chocolate for extra decadence",
                    body: @"Cut 185g unsalted butter into smallish cubes and tip into a medium bowl. Break 185g best dark chocolate into small pieces and drop into the bowl. Fill a small saucepan about a quarter full with hot water, then sit the bowl on top so it rests on the rim of the pan, not touching the water. Put over a low heat until the butter and chocolate have melted, stirring occasionally to mix them. Now remove the bowl from the pan. Alternatively, cover the bowl loosely with cling film and put in the microwave for 2 minutes on High. Leave the melted mixture to cool to room temperature.
While you wait for the chocolate to cool, position a shelf in the middle of your oven and turn the oven on to fan 160C/conventional 180C/gas 4 (most ovens take 10-15 minutes to heat up). Using a shallow 20cm square tin, cut out a square of non-stick baking parchment to line the base. Now tip 85g plain flour and 40g cocoa powder into a sieve held over a medium bowl, and tap and shake the sieve so they run through together and you get rid of any lumps.
With a large sharp knife, chop 50g white chocolate and 50g milk chocolate into chunks on a board. The slabs of chocolate will be quite hard, so the safest way to do this is to hold the knife over the chocolate and press the tip down on the board, then bring the rest of the blade down across the chocolate. Keep on doing this, moving the knife across the chocolate to chop it into pieces, then turn the board round 90 degrees and again work across the chocolate so you end up with rough squares.
Break 3 large eggs into a large bowl and tip in 275g golden caster sugar. With an electric mixer on maximum speed, whisk the eggs and sugar until they look thick and creamy, like a milk shake. This can take 3-8 minutes, depending on how powerful your mixer is, so don’t lose heart. You’ll know it’s ready when the mixture becomes really pale and about double its original volume. Another check is to turn off the mixer, lift out the beaters and wiggle them from side to side. If the mixture that runs off the beaters leaves a trail on the surface of the mixture in the bowl for a second or two, you’re there.
Pour the cooled chocolate mixture over the eggy mousse, then gently fold together with a rubber spatula. Plunge the spatula in at one side, take it underneath and bring it up the opposite side and in again at the middle. Continue going under and over in a figure of eight, moving the bowl round after each folding so you can get at it from all sides, until the two mixtures are one and the colour is a mottled dark brown. The idea is to marry them without knocking out the air, so be as gentle and slow as you like – you don’t want to undo all the work you did in step 4.
Hold the sieve over the bowl of eggy chocolate mixture and resift the cocoa and flour mixture, shaking the sieve from side to side, to cover the top evenly. Gently fold in this powder using the same figure of eight action as before. The mixture will look dry and dusty at first, and a bit unpromising, but if you keep going very gently and patiently, it will end up looking gungy and fudgy. Stop just before you feel you should, as you don’t want to overdo this mixing. Finally, stir in the white and milk chocolate chunks until they’re dotted throughout. Now your mixing is done and the oven can take over.
Pour the mixture into the prepared tin, scraping every bit out of the bowl with the spatula. Gently ease the mixture into the corners of the tin and paddle the spatula from side to side across the top to level it. Put in the oven and set your timer for 25 minutes. When the buzzer goes, open the oven, pull the shelf out a bit and gently shake the tin. If the brownie wobbles in the middle, it’s not quite done, so slide it back in and bake for another 5 minutes until the top has a shiny, papery crust and the sides are just beginning to come away from the tin. Take out of the oven.
Leave the whole thing in the tin until completely cold, then, if you’re using the brownie tin, lift up the protruding rim slightly and slide the uncut brownie out on its base. If you’re using a normal tin, lift out the brownie with the foil. Cut into quarters, then cut each quarter into four squares and finally into triangles. These brownies are so addictive you’ll want to make a second batch before the first is finished, but if you want to make some to hide away for a special occasion, it’s useful to know that they’ll keep in an airtight container for a good two weeks and in the freezer for up to a month.",
                    date: new DateTime(2016, 03, 27),
                    authorUsername: "Meri"
                );

                CreatePost(context,
                    title: "Chicken & chorizo jambalayad",
                    description: "A Cajun-inspired rice pot recipe with spicy Spanish sausage, sweet peppers and tomatoes",
                    body: @"Heat the oil in a large frying pan with a lid and brown the chicken for 5-8 mins until golden. Remove and set aside. Tip in the onion and cook for 3-4 mins until soft. Then add the pepper, garlic, chorizo and Cajun seasoning, and cook for 5 mins more.
Stir the chicken back in with the rice, add the tomatoes and stock. Cover and simmer for 20-25 mins until the rice is tender.",
                    date: new DateTime(2016, 02, 18),
                    authorUsername: "Pesho"
                );

                CreatePost(context,
                    title: "Falafel burgers",
                    description: "A healthy burger that's filling too. These are great for anyone who craves a big bite but doesn't want the calories",
                    body: @"Pat the chickpeas dry with kitchen paper. Tip into a food processor along with the onion, garlic, parsley, spices, flour and a little salt. Blend until fairly smooth, then shape into four patties with your hands.
Heat the oil in a non-stick frying pan, add the burgers, then quickly fry for 3 mins on each side until lightly golden. Serve with toasted pittas, tomato salsa and a green salad.",
                    date: new DateTime(2016, 04, 11),
                    authorUsername: "george"
                );

                CreatePost(context,
                    title: "One-pot chicken chasseur",
                    description: "This French bistro classic is easy to make at home and fabulous with creamy mash or crusty bread",
                    body: @"Heat the oil and half the butter in a large lidded casserole. Season the chicken, then fry for about 5 mins on each side until golden brown. Remove and set aside.
Melt the rest of the butter in the pan. Add the onion, then fry for about 5 mins until soft. Add garlic, cook for about 1 min, add the mushrooms, cook for 2 mins, then add the wine. Stir in the tomato purée, let the liquid bubble and reduce for about 5 mins, then stir in the thyme and pour over the stock. Slip the chicken back into the pan, then cover and simmer on a low heat for about 1 hr until the chicken is very tender.
Remove the chicken from the pan and keep warm. Rapidly boil down the sauce for 10 mins or so until it is syrupy and the flavour has concentrated. Put the chicken legs back into the sauce and serve.",
                    date: new DateTime(2016, 06, 30),
                    authorUsername: "george"
                );

                context.SaveChanges();
            }
        }

        private void CreateUser(ApplicationDbContext context,
            string username, string email, string password, string fullName)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                FullName = fullName
            };

            var userCreateResult = userManager.Create(user, password);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }
        }

        private void CreateRole(ApplicationDbContext context, string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(roleName));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }
        }

        private void AddUserToRole(ApplicationDbContext context, string userName, string roleName)
        {
            var user = context.Users.First(u => u.UserName == userName);
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var addAdminRoleResult = userManager.AddToRole(user.Id, roleName);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }

        private void CreatePost(ApplicationDbContext context,
            string title, string description, string body, DateTime date, string authorUsername)
        {
            var post = new Post();
            post.Title = title;
            post.Description = description;
            post.Body = body;
            post.Date = date;
            post.Author = context.Users.FirstOrDefault(u => u.UserName == authorUsername);
            context.Posts.Add(post);
        }

        private void CreateTag(ApplicationDbContext context , string[] names)
        {
            foreach (var name in names)
            {
                var tag = new Tag
                {
                    Name = name
                };
            }
        }
    }
}