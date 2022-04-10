namespace QLNhaHang.API.Enums
{
    public enum Status
    {
        /// <summary>
        /// Đang chờ
        /// </summary>
        Waiting,
        /// <summary>
        /// Đang chế biến
        /// </summary>
        Processing,
        /// <summary>
        /// Hoàn thành
        /// </summary>
        Completed,
        /// <summary>
        /// Hủy
        /// </summary>
        Cancel,
        /// <summary>
        /// Đã thanh toán
        /// </summary>
        Paid,
        /// <summary>
        /// Chưa thanh toán
        /// </summary>
        Unpaid,
        /// <summary>
        /// Đang hoạt động
        /// </summary>
        Active,
        /// <summary>
        /// Vô hiệu hóa
        /// </summary>
        Disable
    }
}
