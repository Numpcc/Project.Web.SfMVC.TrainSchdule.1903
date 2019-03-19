﻿using PhotoHub.DAL.Entities;
using PhotoHub.BLL.DTO;

namespace PhotoHub.BLL.Extensions
{
    /// <summary>
    /// Methods for mapping comment entities to comment data transfer objects.
    /// </summary>
    public static class CommentsExtensions
    {
        /// <summary>
        /// Maps comment entity to comment DTO without owner.
        /// </summary>
        public static CommentDTO ToDTO(this Comment item, UserDTO owner=null)
        {
            if (item == null)
            {
                return null;
            }

            return new CommentDTO
            {
                Id = item.Id,
                Text = item.Text,
                Owner = owner,
                Date = item.Date
            };
        }

    }
}
