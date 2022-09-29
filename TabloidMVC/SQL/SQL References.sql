ALTER Table Comment
DROP CONSTRAINT [FK_Comment_Post];
ALTER Table Comment
ADD CONSTRAINT  [FK_Comment_Post]
FOREIGN KEY ([PostId]) REFERENCES [Post]([Id]) ON DELETE CASCADE;



