import { observer } from "mobx-react-lite";
import { Profile } from "../../app/models/profile";
import { Button, Reveal } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";

interface Props {
  profile: Profile;
}
const FollowButton = ({ profile }: Props) => {
  const { profileStore, userStore } = useStore();
  const { updateFollowing, loading } = profileStore;

  function handleFollow(e: React.SyntheticEvent, username: string) {
    e.preventDefault();
    profile.following
      ? updateFollowing(username, false)
      : updateFollowing(username, true);
  }
  if (userStore.user?.username === profile.username) return null;
  return (
    <Reveal animated="move up">
      <Reveal.Content visible style={{ width: "100%" }}>
        <Button
          fluid
          color="teal"
          content={profile.following ? "Following" : "Not Following"}
        />
      </Reveal.Content>
      <Reveal.Content hidden style={{ width: "100%" }}>
        <Button
          fluid
          basic
          color={profile.following ? "red" : "green"}
          content={profile.following ? "Unfollow" : "Follow"}
          onClick={(e) => handleFollow(e, profile.username)}
          loading={loading}
          disabled={loading}
        />
      </Reveal.Content>
    </Reveal>
  );
};
export default observer(FollowButton);
