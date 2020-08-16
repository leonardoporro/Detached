﻿using Detached.Annotations;
using Detached.Mapping;
using Detached.Mapping.Context;
using Xunit;

namespace Detached.Tests.Mapping.Entity
{
    public class EntityAssociatedMapperTests
    {
        Mapper mapper = new Mapper();

        [Fact]
        public void load_associated_when_key_doesnt_match()
        {
            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "target",
                Associated = new TargetAssociated
                {
                    Id = 1,
                    Name = "target_associated"
                }
            };

            int targetCheck = target.Associated.GetHashCode();

            SourceEntity source = new SourceEntity
            {
                Id = 1,
                Name = "source",
                Associated = new SourceAssociated
                {
                    Id = 2,
                    Name = "source_associated"
                }
            };

            MapperContext context = new MapperContext();

            var mapped = mapper.Map(source, target, context);

            Assert.NotNull(mapped.Associated);
            Assert.Null(mapped.Associated.Name); // properties are not mapped
            Assert.Equal(2, mapped.Associated.Id);// key is mapped
            Assert.NotEqual(targetCheck, mapped.Associated.GetHashCode());

            // there is no tracking for disposed entity.
            Assert.False(context.TryGetEntry<TargetAssociated>(new EntityKey<int>(1), out MapperContextEntry entry)); 

            // new entity is loaded.
            Assert.True(context.TryGetEntry<TargetAssociated>(new EntityKey<int>(2), out MapperContextEntry entry2));
            Assert.Equal(MapperActionType.Attach, entry2.ActionType);
        }

        [Fact]
        public void load_associated_when_target_is_null()
        {
            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "target",
                Associated = null
            };

            SourceEntity source = new SourceEntity
            {
                Id = 1,
                Name = "source",
                Associated = new SourceAssociated
                {
                    Id = 2,
                    Name = "source_associated"
                }
            };

            MapperContext context = new MapperContext();

            var mapped = mapper.Map(source, target, context);

            Assert.NotNull(mapped.Associated);
            Assert.Null(mapped.Associated.Name); // properties are not mapped
            Assert.Equal(2, mapped.Associated.Id);// key is mapped

            // there is no tracking for disposed entity.
            Assert.False(context.TryGetEntry<TargetAssociated>(new EntityKey<int>(1), out MapperContextEntry entry));

            // new entity is loaded.
            Assert.True(context.TryGetEntry<TargetAssociated>(new EntityKey<int>(2), out MapperContextEntry entry2));
            Assert.Equal(MapperActionType.Attach, entry2.ActionType);
        }

        [Fact]
        public void dont_load_associated_when_key_matches()
        {
            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "target",
                Associated = new TargetAssociated
                {
                    Id = 1,
                    Name = "target_associated"
                }
            };

            int targetCheck = target.Associated.GetHashCode();

            SourceEntity source = new SourceEntity
            {
                Id = 1,
                Name = "source",
                Associated = new SourceAssociated
                {
                    Id = 1,
                    Name = "source_associated"
                }
            };

            MapperContext context = new MapperContext();

            var mapped = mapper.Map(source, target, context);

            Assert.NotNull(mapped.Associated);
            Assert.Equal(1, mapped.Associated.Id);// key is mapped
            Assert.Equal(targetCheck, mapped.Associated.GetHashCode());
 
            Assert.False(context.TryGetEntry<TargetAssociated>(new EntityKey<int>(1), out MapperContextEntry entry2));
        }
 
        [Entity]
        public class TargetEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public TargetAssociated Associated { get; set; }
        }

        [Entity]
        public class TargetAssociated
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }


        public class SourceEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public SourceAssociated Associated { get; set; }
        }

        public class SourceAssociated
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}