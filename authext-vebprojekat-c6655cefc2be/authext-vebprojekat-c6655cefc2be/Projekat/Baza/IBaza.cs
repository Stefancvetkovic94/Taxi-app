using System;
using Projekat.Models;
using System.Collections.Generic;

namespace Projekat.Baza
{
    public interface IBaza
    {
        // User related
        IEnumerable<Theme> GetSavedThemes(string username);

        bool SaveTheme(string username, string subforum, string theme);
        string Recommended(string username);
        string GetSubFromTheme(string tema);
        bool CanLogIn(string username, string password);
        bool Register(Korisnik user);
        bool AddModerator(string subforum, string newmod);
        bool RemoveModerator(string subforum, string mod);
        bool ChangeRole(string username, User.ForumRole role);
        bool CanDeleteSubforum(string username, string subforum);
        bool CanEditOrDeleteTheme(string username, string subforum, string title);

        Korisnik GetUser(string username);
        IEnumerable<ThemeVote> GetUserThemeVotes(string username);
        IEnumerable<CommentVote> GetUserCommentVotes(string username);
       
        bool CanEditComment(string username, uint id);
        bool CanCreateSubforum(string username);
       
        bool CanDeleteComment(string username, uint id);
        bool CanAddOrRemoveModerator(string username, string subforum);

        // Message related
        IEnumerable<FromMessage> GetMessagesFrom(string username);
        IEnumerable<ToMessage> GetMessagesTo(string username);
        Message GetMessage(uint id);
        bool SendMessage(string from, string to, string content);

        // Subforum related
        IEnumerable<string> GetSubforumNames();
        Subforum GetSubforum(string name);
        bool CreateSubforum(string username, CreationSubforum subforum);
        bool DeleteSubforum(string subforum);

        bool DeleteSavedThemes(string theme);


        List<string> GetThemeNamesFromSubforum(string subforum);


      
        Theme GetTheme(string subforum, string title);
        bool CreateTheme(string subforum, CreationTheme theme);
        bool EditTheme(string subforum, string title, Kind kind, string content);
        bool DeleteTheme(string subforum, string title);
        bool DeleteComents(string subforum, string title);
        bool DeleteVotes(string title);
        bool DeleteThemeCommentsVotes(string title);



        bool VoteTheme(string username, string subforum, string title, bool isPositive);
        IEnumerable<SubforumSearchResult> SearchSubforums(SearchSubforums search);
        IEnumerable<ThemeSearchResult> SearchThemes(SearchThemes search);
        IEnumerable<UserSearchResult> SearchUsers(SearchUsers search);

        bool AddThemeComment(ReplyThemeComment comment);
        bool AddComment(ReplyComment comment);
        ReplyVisualizerComment GetComment(uint id);
        bool EditComment(string username, uint id, EditComment comment);
        bool DeleteComment(uint id);
        bool VoteComment(string username, uint id, bool isPositive);


        bool EditUser(string korisnickoime, string ime, string prezime, string telefon, string pol, string jmbg, string email);


    }
}